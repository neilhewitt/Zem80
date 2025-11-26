using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Drawing.Text;
using Zem80.Core.CPU;
using Zem80.Core.Debugger;
using ZXSpectrum.VM;

namespace ZXSpectrum_MAUI;

public partial class DisplayPage : ContentPage
{
    private Spectrum48K _vm;
    private bool _isClosing = false;
    private SKBitmap _currentBitmap;
    private readonly object _bitmapLock = new object();

    private bool _isDebuggerVisible = false;
    private bool _breakpointReached = false;
    private bool _breakpointEntryHasFocus = false;
    private bool _canvasHasFocus = false;
    private bool _debuggerPaneHasFocus = false;
    private bool _waitingForNextInstructionButton = false;
    private bool _jumped;

    private Label _lastDebuggerLine;
    private Label _currentDebuggerLine;

    private DebugSession _debugSession;

    // Track previous display values for highlighting changes
    private Dictionary<string, byte> _previousByteRegisterValues = new Dictionary<string, byte>();
    private Dictionary<string, ushort> _previousWordRegisterValues = new Dictionary<string, ushort>();
    private Dictionary<string, bool> _previousFlagValues = new Dictionary<string, bool>();

    private ushort? _runningToAddress = null; // null means not running to any address
    private const int MAX_DEBUGGER_LINES = 1000;
    
    // Circular buffer for debugger output
    private readonly Label[] _debuggerLabels = new Label[MAX_DEBUGGER_LINES];
    private int _debuggerBufferIndex = 0;
    private int _debuggerItemCount = 0;

    public bool DebuggerVisible
    {
        get => _isDebuggerVisible;
        set
        {
            if (_isDebuggerVisible != value)
            {
                _isDebuggerVisible = value;
                if (value) _vm.Mute(); // stop crackling in debug
                else _vm.Unmute();

                // use Dispatcher to ensure UI updates happen on the main thread in case we get called from another thread
                Dispatcher.Dispatch(() =>
                    {
                        DebuggerPane.IsVisible = value;
                        UpdateWindowSize();
                    });
            }
        }
    }

    public DisplayPage()
    {
        InitializeComponent();

        // Pre-create all labels for the circular buffer
        for (int i = 0; i < MAX_DEBUGGER_LINES; i++)
        {
            var label = new Label
            {
                FontFamily = "Consolas,Courier New,monospace",
                FontSize = 12,
                Padding = new Thickness(5, 0),
                IsVisible = false // Start hidden
            };
            _debuggerLabels[i] = label;
            DebuggerOutputStack.Add(label);
        }

        // Track focus for breakpoint and run-to entries to prevent keys going to Spectrum
        BreakpointAddressEntry.Focused += (s, e) => 
        { 
            _breakpointEntryHasFocus = true; 
            UpdateDebuggerHeaderFocusState(false); // Breakpoint entry is in left pane, not debugger
        };
        BreakpointAddressEntry.Unfocused += (s, e) => { _breakpointEntryHasFocus = false; };
        RunToAddressEntry.Focused += (s, e) => 
        { 
            _breakpointEntryHasFocus = true; 
            UpdateDebuggerHeaderFocusState(true); // Run-to entry is in debugger pane
        };
        RunToAddressEntry.Unfocused += (s, e) => 
        { 
            _breakpointEntryHasFocus = false; 
            UpdateDebuggerHeaderFocusState(false);
        };

        // Track focus for scroll view in debugger pane
        DebuggerScrollView.Focused += (s, e) => UpdateDebuggerHeaderFocusState(true);
        DebuggerScrollView.Unfocused += (s, e) => UpdateDebuggerHeaderFocusState(false);

        BreakpointAddressEntry.Completed += (s, e) => OnAddBreakpointClicked(s, e);
        RunToAddressEntry.Completed += (s, e) => OnRunToAddressClicked(s, e);

        // MAUI does not have Loaded, Focusable, Focused, Unfocused, KeyDown, KeyUp on ContentPage.
        // Use Appearing event and attach keyboard events to a view that can receive focus, e.g., SKCanvasView.
        this.Appearing += DisplayPage_Appearing;
    }

    private void UpdateWindowSize()
    {
        // Update the window size based on debugger visibility
        var window = Application.Current?.Windows[0];
        if (window != null)
        {
            // Measure the actual height of the left pane (canvas + breakpoint controls)
            // Canvas is 512px, breakpoint controls are approximately 40px (reduced padding + smaller controls)
            int canvasHeight = 512;
            int breakpointControlsHeight = 40; // Approximate height for the smaller controls section
            int totalHeight = canvasHeight + breakpointControlsHeight;

            // Update debugger pane to match the left pane height
            DebuggerPane.HeightRequest = totalHeight;

            // Base width is just the canvas width
            int baseWidth = 640;
            int debuggerWidth = _isDebuggerVisible ? 640 : 0;
            int totalWidth = baseWidth + debuggerWidth;

            window.Width = totalWidth;
            window.Height = totalHeight + 40; // Extra for title bar
        }
    }

    private void AddDebuggerOutput(string message)
    {
        Label label = _debuggerLabels[_debuggerBufferIndex]; 

        Dispatcher.Dispatch(() =>
        {
            // Update the label with new content
            label.FormattedText = FormattedDebugMessageConverter.Convert(message, System.Globalization.CultureInfo.CurrentCulture);
            label.IsVisible = true;
            
            // Move to next position in circular buffer
            _debuggerBufferIndex = (_debuggerBufferIndex + 1) % MAX_DEBUGGER_LINES;
            
            // Track how many items we've added (up to MAX_DEBUGGER_LINES)
            if (_debuggerItemCount < MAX_DEBUGGER_LINES)
            {
                _debuggerItemCount++;
            }
            else
            {
                // When buffer is full, the oldest item is now at _debuggerBufferIndex
                // We need to move it to the end of the visual stack for proper ordering
                var oldestLabel = _debuggerLabels[_debuggerBufferIndex];
                DebuggerOutputStack.Remove(oldestLabel);
                DebuggerOutputStack.Add(oldestLabel);
            }
            
            // Auto-scroll to bottom
            DebuggerScrollView.ScrollToAsync(0, DebuggerOutputStack.Height, false);
        });

        _lastDebuggerLine = _currentDebuggerLine;
        _currentDebuggerLine = label;
    }

    private void ClearDebuggerOutput()
    {
        Dispatcher.Dispatch(() =>
        {
            // Hide all labels and reset the circular buffer
            for (int i = 0; i < MAX_DEBUGGER_LINES; i++)
            {
                _debuggerLabels[i].IsVisible = false;
                _debuggerLabels[i].FormattedText = null;
            }
            
            _debuggerBufferIndex = 0;
            _debuggerItemCount = 0;
        });
    }

    private void UpdateLastDebuggerLine(string message)
    {
        Dispatcher.Dispatch(() =>
        {
            if (_debuggerItemCount == 0)
            {
                return; // No lines to update
            }

            var label = _lastDebuggerLine;

            string existingMarkup = FormattedDebugMessageConverter.Convert(label.FormattedText, System.Globalization.CultureInfo.CurrentCulture);
            string plainText = FormattedDebugMessageConverter.ConvertToPlainText(existingMarkup);
            int padding = 30 - plainText.Length;

            string updatedMessage = existingMarkup + new string(' ', padding) + message;

            label.FormattedText = FormattedDebugMessageConverter.Convert(updatedMessage, System.Globalization.CultureInfo.CurrentCulture);
        });
    }

    //private void BreakpointReached(IBreakpointInfo breakpointInfo)   
    //{
    //    _breakpointReached = true;
        
    //    // Automatically give focus to the debugger pane when breakpoint is hit
    //    Dispatcher.Dispatch(() =>
    //    {
    //        UpdateDebuggerHeaderFocusState(true);
            
    //        // Reset register/flag change tracking when first entering debug mode
    //        _previousWordRegisterValues.Clear();
    //        _previousFlagValues.Clear();
            
    //        // Actually set keyboard focus to the scroll view so Enter key works immediately
    //        DebuggerScrollView.Focus();
    //    });
    //}

    private async void DisplayPage_Appearing(object sender, EventArgs e)
    {
        // Set initial window size
        UpdateWindowSize();

        _vm = new Spectrum48K(UpdateDisplay);
        _vm.CPU.Debug.SubscribeToDebug(BeginDebugging);
        _vm.CPU.BeforeExecuteInstruction += CPU_BeforeExecuteInstruction;
        _vm.CPU.AfterExecuteInstruction += CPU_AfterExecuteInstruction;

        //var result = await FilePicker.Default.PickAsync(new PickOptions
        //{
        //    PickerTitle = "Select ZX Spectrum Snapshot",
        //    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        //    {
        //        { DevicePlatform.WinUI, new[] { ".sna", ".z80" } }
        //    })
        //});

        //if (result != null)
        //{
        //    _vm.Start(result.FullPath);
        //}
        //else
        //{
            _vm.Start();
        //}

#if WINDOWS
        SetupWindowsKeyboardHandling();
#endif
    }

    private void BeginDebugging(ushort breakpointAddress, DebugSession debugSession)
    {
        _breakpointReached = true;
        _debugSession = debugSession;

        DebugMonitor beforeInstructionMonitor =_debugSession.Monitor(DebugEventTypes.BeforeInstructionExecution, state =>
        {
            return DebugResponse.StepNext();
        });

        DebugMonitor afterInstructionMonitor = _debugSession.Monitor(DebugEventTypes.AfterInstructionExecution, state =>
        {
            return DebugResponse.StepNext();
        });

        // Automatically give focus to the debugger pane when breakpoint is hit
        Dispatcher.Dispatch(() =>
        {
            UpdateDebuggerHeaderFocusState(true);
            // Reset register/flag change tracking when first entering debug mode
            _previousByteRegisterValues.Clear();
            _previousWordRegisterValues.Clear();
            _previousFlagValues.Clear();
            // Actually set keyboard focus to the scroll view so Enter key works immediately
            DebuggerScrollView.Focus();
        });
    }

    private void CPU_BeforeExecuteInstruction(object sender, InstructionPackage instructionPackage)
    {
        if (!DebuggerVisible || !_breakpointReached || _vm?.CPU == null) return;

        (ushort address, string disassembly) = instructionPackage.Disassemble();

        AddDebuggerOutput($"<color:{(_jumped ? "green" : "darkgray")}>0x{address.ToString("X4")}</color> <color:white>{disassembly}</color>");
        if (_jumped) _jumped = false;

        if (_runningToAddress.HasValue && _runningToAddress != instructionPackage.InstructionAddress) return;

        _waitingForNextInstructionButton = true;
        while (_waitingForNextInstructionButton)
        {
            // allow all other UI events to process
            System.Threading.Thread.Sleep(1);
        }
    }

    private void CPU_AfterExecuteInstruction(object sender, ExecutionResult executionResult)
    {
        if (!DebuggerVisible || !_breakpointReached || _vm?.CPU == null) return;

        _jumped = executionResult.ProgramCounterWasModified;

        Dispatcher.Dispatch(() =>
        {
            var registers = _vm.CPU.Registers;
            var flags = _vm.CPU.Flags;

            string changedRegisters = "";

            if (executionResult.Instruction.TargetsByteRegister)
            {
                foreach (var byteRegister in Enum.GetValues<ByteRegister>())
                {
                    if (byteRegister == ByteRegister.None) continue;
                    
                    byte currentValue = registers[byteRegister];
                    string registerName = byteRegister.ToString();
                    
                    if (_previousByteRegisterValues.TryGetValue(registerName, out byte previousValue))
                    {
                        if (previousValue != currentValue)
                        {
                            changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:darkgray>{previousValue:X2}</color>-><color:green>{currentValue:X2}</color></bold>,";
                            _previousByteRegisterValues[registerName] = previousValue;
                        }
                        else if (byteRegister == executionResult.Instruction.Target.AsByteRegister())
                        {
                            changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:white>{previousValue:X2}</color></bold>,";
                        }
                    }
                }
            }
            else if (executionResult.Instruction.TargetsWordRegister)
            {
                foreach (var wordRegister in Enum.GetValues<WordRegister>())
                {
                    if (wordRegister == WordRegister.None) continue;
                    
                    ushort currentValue = registers[wordRegister];
                    string registerName = wordRegister.ToString();

                    if (_previousWordRegisterValues.TryGetValue(registerName, out ushort previousValue))
                    {
                        if (previousValue != currentValue)
                        {
                            if (wordRegister != WordRegister.PC)
                            {
                                changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:darkgray>{previousValue}</color>-><color:green>{currentValue:X4}</color></bold>,";
                            }
                        }
                        else if (wordRegister == executionResult.Instruction.Target.AsWordRegister())
                        {
                            changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:white>{previousValue:X4}</color></bold>,";
                        }
                    }
                }
            }

            changedRegisters = changedRegisters.TrimEnd(',');
            if (changedRegisters.Length > 0)
            {
                UpdateLastDebuggerLine(changedRegisters);
            }

            UpdateRegisterDisplay("AF", ValueAF, registers.AF);
            UpdateRegisterDisplay("BC", ValueBC, registers.BC);
            UpdateRegisterDisplay("DE", ValueDE, registers.DE);
            UpdateRegisterDisplay("HL", ValueHL, registers.HL);
            UpdateRegisterDisplay("IX", ValueIX, registers.IX);
            UpdateRegisterDisplay("IY", ValueIY, registers.IY);
            UpdateRegisterDisplay("SP", ValueSP, registers.SP);
            UpdateRegisterDisplay("PC", ValuePC, registers.PC);
            UpdateRegisterDisplay("IR", ValueIR, registers.IR);

            UpdateFlagDisplay("S", FlagS, flags.Sign);
            UpdateFlagDisplay("Z", FlagZ, flags.Zero);
            UpdateFlagDisplay("Y", FlagY, flags.Y);
            UpdateFlagDisplay("H", FlagH, flags.HalfCarry);
            UpdateFlagDisplay("X", FlagX, flags.X);
            UpdateFlagDisplay("P", FlagP, flags.ParityOverflow);
            UpdateFlagDisplay("N", FlagN, flags.Subtract);
            UpdateFlagDisplay("C", FlagC, flags.Carry);
        });
    }

    private void UpdateRegisterDisplay(string registerName, Label label, byte newValue)
    {
        if (_previousByteRegisterValues.TryGetValue(registerName, out byte previousValue))
        {
            label.TextColor = (previousValue != newValue) ? Colors.Green : Colors.White;
        }
        else
        {
            label.TextColor = Colors.White;
        }

        label.Text = newValue.ToString("X2");
        _previousByteRegisterValues[registerName] = newValue;
    }

    private void UpdateRegisterDisplay(string registerName, Label label, ushort newValue)
    {
        if (registerName != "PC" && _previousWordRegisterValues.TryGetValue(registerName, out ushort previousValue))
        {
            label.TextColor = (previousValue != newValue) ? Colors.Green : Colors.White;
        }
        else
        {
            label.TextColor = Colors.White;
        }

        label.Text = newValue.ToString("X4");
        _previousWordRegisterValues[registerName] = newValue;
    }

    private void UpdateFlagDisplay(string flagName, Label label, bool newValue)
    {
        if (_previousFlagValues.TryGetValue(flagName, out bool previousValue))
        {
            label.TextColor = (previousValue != newValue) ? Colors.Green : Colors.White;
        }
        else
        {
            label.TextColor = Colors.White;
        }

        label.Text = newValue ? "1" : "0";
        _previousFlagValues[flagName] = newValue;
    }

#if WINDOWS
    private void SetupWindowsKeyboardHandling()
    {
        // Get the native Windows window
        var window = Application.Current?.Windows[0];
        if (window?.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
        {
            // Subscribe to key events on the native window content
            var content = nativeWindow.Content as Microsoft.UI.Xaml.FrameworkElement;
            if (content != null)
            {
                content.KeyDown += (s, e) =>
                {
                    // Check if Enter is pressed while debugger is visible and waiting for next instruction
                    // BUT only if the canvas doesn't have focus (so emulator can still receive Enter)
                    if (_waitingForNextInstructionButton && 
                        !_breakpointEntryHasFocus && 
                        !_canvasHasFocus &&
                        e.Key == Windows.System.VirtualKey.Enter)
                    {
                        // Trigger Next Instruction button
                        Dispatcher.Dispatch(() => OnNextInstructionClicked(this, EventArgs.Empty));
                        e.Handled = true;
                        return;
                    }

                    // Only send keys to Spectrum if:
                    // - Breakpoint entry doesn't have focus
                    // - Not waiting for next instruction (in stepping mode)
                    // OR canvas has explicit focus (user clicked on it)
                    if (!_breakpointEntryHasFocus && (!_waitingForNextInstructionButton || _canvasHasFocus))
                    {
                        SpectrumKeyboard.MauiKeyDown((int)e.Key);
                    }
                };

                content.KeyUp += (s, e) =>
                {
                    // Same logic as KeyDown
                    if (!_breakpointEntryHasFocus && (!_waitingForNextInstructionButton || _canvasHasFocus))
                    {
                        SpectrumKeyboard.MauiKeyUp((int)e.Key);
                    }
                };
            }
        }
    }
#endif

    private void UpdateDisplay(byte[] rgba)
    {
        if (!_isClosing)
        {
            lock (_bitmapLock)
            {
                _currentBitmap?.Dispose();
                _currentBitmap = new SKBitmap(320, 256, SKColorType.Bgra8888, SKAlphaType.Opaque);

                unsafe
                {
                    fixed (byte* ptr = rgba)
                    {
                        _currentBitmap.SetPixels((IntPtr)ptr);
                    }
                }
            }

            Dispatcher.Dispatch(() =>
            {
                DisplayCanvas.InvalidateSurface();
            });
        }
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.Black);

        lock (_bitmapLock)
        {
            if (_currentBitmap != null)
            {
                var info = e.Info;
                var destRect = new SKRect(0, 0, info.Width, info.Height);
                canvas.DrawBitmap(_currentBitmap, destRect);
            }
        }
    }

    private void OnAddressTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string newText = e.NewTextValue ?? string.Empty;

        // Remove any invalid characters
        string validText = "";
        bool hasPrefix = newText.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ||
                        newText.StartsWith("0X", StringComparison.OrdinalIgnoreCase);

        for (int i = 0; i < newText.Length; i++)
        {
            char c = newText[i];

            // Allow '0x' prefix at the start
            if (i == 0 && (c == '0'))
            {
                validText += c;
            }
            else if (i == 1 && validText == "0" && (c == 'x' || c == 'X'))
            {
                validText += c;
            }
            // Allow hex digits (0-9, A-F, a-f)
            else if ((c >= '0' && c <= '9') ||
                     (c >= 'A' && c <= 'F') ||
                     (c >= 'a' && c <= 'f'))
            {
                validText += c;
            }
        }

        // Enforce length limits: 4 chars without prefix, 6 chars with prefix (0xFFFF)
        int maxLength = hasPrefix ? 6 : 4;
        if (validText.Length > maxLength)
        {
            validText = validText.Substring(0, maxLength);
        }

        // Only update if the text changed
        if (validText != newText)
        {
            entry.Text = validText;
        }
    }

    private async void OnAddBreakpointClicked(object sender, EventArgs e)
    {
        string addressText = BreakpointAddressEntry.Text?.Trim();

        if (string.IsNullOrEmpty(addressText))
        {
            return;
        }

        try
        {
            // Parse the hex address (with or without 0x prefix)
            ushort address;
            if (addressText.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                address = Convert.ToUInt16(addressText.Substring(2), 16);
            }
            else
            {
                address = Convert.ToUInt16(addressText, 16);
            }

            // Add the breakpoint
            _vm?.CPU?.Debug?.AddBreakpoint(address);

            // Clear the entry
            BreakpointAddressEntry.Text = "";

            DebuggerVisible = true;
            AddDebuggerOutput($"<bold><color:white>Breakpoint added at <color:green>0x{address:X4}</color></color></bold>");
        }
        catch (Exception ex)
        {
            // swallow this
        }
    }

    private void OnRunToAddressClicked(object sender, EventArgs e)
    {
        string addressText = RunToAddressEntry.Text?.Trim();

        if (string.IsNullOrEmpty(addressText))
        {
            return;
        }

        try
        {
            ushort address;
            if (addressText.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                address = Convert.ToUInt16(addressText.Substring(2), 16);
            }
            else
            {
                address = Convert.ToUInt16(addressText, 16);
            }

            _runningToAddress = address;

            RunToAddressEntry.Text = "";
        }
        catch (Exception ex)
        {
            // swallow this
        }
    }

    private void OnNextInstructionClicked(object sender, EventArgs e)
    {
        _waitingForNextInstructionButton = false;
    }

    protected override void OnDisappearing()
    {
        _isClosing = true;
        _vm?.Stop();

        lock (_bitmapLock)
        {
            _currentBitmap?.Dispose();
            _currentBitmap = null;
        }

        base.OnDisappearing();
    }

    private void UpdateDebuggerHeaderFocusState(bool isFocused)
    {
        _debuggerPaneHasFocus = isFocused;
        
        if (isFocused)
        {
            // Invert colors when focused
            DebuggerHeader.BackgroundColor = Color.FromArgb("#007ACC");
        }
        else
        {
            // Default colors when unfocused
            DebuggerHeader.BackgroundColor = Color.FromArgb("#3D3D40");
        }
    }

    private void OnCanvasTapped(object sender, EventArgs e)
    {
        // User clicked on the canvas - they want to interact with the emulator
        _canvasHasFocus = true;
        
        // Canvas gaining focus means debugger loses focus
        UpdateDebuggerHeaderFocusState(false);
    }

    private void OnDebuggerPaneTapped(object sender, EventArgs e)
    {
        // User clicked in the debugger pane - they want to interact with the debugger
        _canvasHasFocus = false;
        UpdateDebuggerHeaderFocusState(true);
    }
}
