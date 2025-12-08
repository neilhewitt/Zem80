using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Drawing.Text;
using System.Net;
using System.Threading.Tasks;
using Zem80.Core.CPU;
using Zem80.Core.Debugger;
using ZXSpectrum.VM;
using ZXSpectrum_MAUI.Settings;

namespace ZXSpectrum_MAUI;

public partial class DisplayPage : ContentPage
{
    private Spectrum48K _vm;
    private bool _isClosing = false;
    private SKBitmap _currentBitmap;
    private readonly object _bitmapLock = new object();

    private bool _isDebuggerVisible = false;
    private bool _sendKeysToEmulator = true;
    private bool _waitingForNextInstructionButton = false;
    private bool _jumped;
    private bool _firstDebug;
    private bool _debuggingStopped;

    private DebugSession _debugSession;
    private SettingsManager _settingsManager;

    // Track previous display values for highlighting changes
    private Dictionary<string, byte> _previousByteRegisterValues = new Dictionary<string, byte>();
    private Dictionary<string, ushort> _previousWordRegisterValues = new Dictionary<string, ushort>();
    private Dictionary<string, bool> _previousFlagValues = new Dictionary<string, bool>();

    // Circular buffer for debugger output
    private int MAX_DEBUGGER_LINES = 100;
    private Label[] _debuggerLabels;
    private int _debuggerBufferIndex = 0;
    private int _lastDebuggerBufferIndex = 0;
    private int _debuggerItemCount = 0;

    public bool DebuggerVisible
    {
        get => _isDebuggerVisible;
        set
        {
            if (_isDebuggerVisible != value)
            {
                _isDebuggerVisible = value;
                
                var settings = _settingsManager?.Settings ?? new AppSettings();
                
                if (value && settings.MuteWhenDebugging) 
                {
                    _vm.Mute(); // stop crackling in debug
                }
                else if (!value)
                {
                    _vm.Unmute();
                }

                // use Dispatcher to ensure UI updates happen on the main thread in case we get called from another thread
                Dispatcher.Dispatch(() =>
                    {
                        DebuggerPane.IsVisible = value;
                        UpdateWindowSize();
                    });
            }
        }
    }

    private void UpdateWindowSize()
    {
        var settings = _settingsManager?.Settings ?? new AppSettings();

        // Update the window size based on debugger visibility
        var window = Application.Current?.Windows[0];
        if (window != null)
        {            
            // Measure the actual height of the left pane (canvas + breakpoint controls)
            int canvasHeight = 512;
            int breakpointControlsHeight = 40; // Approximate height for the smaller controls section
            int totalHeight = canvasHeight + (settings.DebuggerAvailable ? breakpointControlsHeight : 0);

            // Update debugger pane to match the left pane height
            DebuggerPane.HeightRequest = totalHeight;

            // Base width is the canvas width
            int baseWidth = 640;
            int debuggerWidth = _isDebuggerVisible ? 640 : 0;
            int totalWidth = baseWidth + debuggerWidth;

            window.Width = totalWidth;
            window.Height = totalHeight + 40; // Extra for title bar
        }
    }

    private void AddDebuggerOutput(string message)
    {
        Thread.Sleep(1); // ensure ordering
        Label label = _debuggerLabels[_debuggerBufferIndex]; ;

        Dispatcher.Dispatch(() =>
        {
            // Update the label with new content
            label.FormattedText = FormattedDebugMessageConverter.Convert(message, System.Globalization.CultureInfo.CurrentCulture);
            label.IsVisible = true;
            
            // Move to next position in circular buffer
            _lastDebuggerBufferIndex = _debuggerBufferIndex;
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
            
            DebuggerScrollView.ScrollToAsync(0, DebuggerOutputStack.Height, false);
        });
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

            var label = _debuggerLabels[_lastDebuggerBufferIndex];

            string existingMarkup = FormattedDebugMessageConverter.Convert(label.FormattedText, System.Globalization.CultureInfo.CurrentCulture);
            string plainText = FormattedDebugMessageConverter.ConvertToPlainText(existingMarkup);
            int padding = 30 - plainText.Length;
            if (padding < 0) padding = 0;

            string updatedMessage = existingMarkup + new string(' ', padding) + message;

            label.FormattedText = FormattedDebugMessageConverter.Convert(updatedMessage, System.Globalization.CultureInfo.CurrentCulture);
        });
    }

    private async void DisplayPage_Appearing(object sender, EventArgs e)
    {
        // Set initial window size
        UpdateWindowSize();

        _vm = new Spectrum48K(UpdateSpectrumDisplay);
        _vm.CPU.Debug.OnBreakpointReached += OnBeginDebugging;
        _vm.CPU.Debug.OnDebugSessionEnded += (sender, state) =>
        {
            _debuggingStopped = true;
            Dispatcher.Dispatch(() =>
            {
                DebuggerHeader.BackgroundColor = Color.FromArgb("#3D3D40"); // blue
            });
        };

        var settings = _settingsManager?.Settings ?? new AppSettings();
        if (!settings.DebuggerAvailable)
        {
            DebuggerPane.IsVisible = false;
            BreakpointControls.IsVisible = false;
        }

        if (settings.ShowFilePickerOnStartup)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select ZX Spectrum Snapshot",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".sna", ".z80" } }
                })
                
            });

            if (result != null)
            {
                _vm.Start(result.FullPath);
            }
            else
            {
                _vm.Start();
            }
        }
        else
        {
            _vm.Start();
        }

#if WINDOWS
        SetupWindowsKeyboardHandling();
#endif
    }

    private void UpdateSpectrumDisplay(byte[] rgba)
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

    private void OnBeginDebugging(object sender, DebugSession debugSession)
    {
        _debugSession = debugSession;
        _firstDebug = true;

        _debugSession.Monitor(DebugEventTypes.BeforeInstructionExecution, state =>
        {
            return Debug_BeforeExecuteInstruction(state);
        });

        _debugSession.Monitor(DebugEventTypes.AfterInstructionExecution, state =>
        {
            return Debug_AfterExecuteInstruction(state);
        });

        // Automatically give focus to the debugger pane when breakpoint is hit
        Dispatcher.Dispatch(() =>
        {
            _previousByteRegisterValues.Clear();
            _previousWordRegisterValues.Clear();
            _previousFlagValues.Clear();

            DebuggerHeader.BackgroundColor = Color.FromArgb("#005500");
            DebuggerScrollView.Focus();
        });
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

    private void OnAddBreakpointClicked(object sender, EventArgs e)
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
            _vm.CPU.Debug.AddBreakpoint(address);

            // Clear the entry
            BreakpointAddressEntry.Text = "";

            DebuggerVisible = true;
            AddDebuggerOutput($"<bold><color:white>Breakpoint added at <color:green>0x{address:X4}</color></color></bold>");
        }
        catch (Exception)
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

            RunToAddressEntry.Text = "";
            AddDebuggerOutput($"<bold><color:white>Running to address <color:green>0x{address:X4}</color></color></bold>");

            _vm.CPU.Debug.AddBreakpoint(address);
            _debuggingStopped = true;
            _waitingForNextInstructionButton = false;
        }
        catch (Exception)
        {
            // swallow this
        }
    }

    private void OnNextInstructionClicked(object sender, EventArgs e)
    {
        _waitingForNextInstructionButton = false;
    }

    private void OnBreakNowClicked(object sender, EventArgs e)
    {
        if (_vm.CPU.Debug.IsDebugging)
        {
            return; // already debugging
        }

        DebuggerVisible = true;
        _vm.CPU.Debug.BreakNow();
    }

    private async void OnStopDebuggingClicked(object sender, EventArgs e)
    {
        if (_vm.CPU.Debug.IsDebugging)
        {
            _debuggingStopped = true; // causes Debug_AfterExecuteInstruction to return Stop
            _waitingForNextInstructionButton = false;

            AddDebuggerOutput($"<bold><color:white>Debugging stopped: </color></color></bold>");
            await Task.Delay(3000);
            DebuggerVisible = false;
            ClearDebuggerOutput();
        }
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
                bool sendKeysToEmulator = !_settingsManager.Settings.DebuggerAvailable;
                if (!sendKeysToEmulator)
                {
                    sendKeysToEmulator = !_isDebuggerVisible || !_waitingForNextInstructionButton || _sendKeysToEmulator;
                }

                content.KeyDown += (s, e) =>
                {
                    VirtualKey key = (VirtualKey)e.Key; // need to convert to our local VirtualKey even though the values are identical

                    if (sendKeysToEmulator)
                    {
                        SpectrumKeyboard.SendKeyDown(key);
                        return;
                    }
                    else if (_waitingForNextInstructionButton)
                    {
                        if (key == VirtualKey.Enter)
                        {
                            // Trigger Next Instruction button
                            Dispatcher.Dispatch(() => OnNextInstructionClicked(this, EventArgs.Empty));
                            e.Handled = true;
                            return;
                        }
                        else if (key == VirtualKey.Escape)
                        {
                            _debuggingStopped = true;
                            _waitingForNextInstructionButton = false;
                            e.Handled = true;
                            return;
                        }
                    }
                };

                content.KeyUp += (s, e) =>
                {
                    // Same logic as KeyDown
                    if (sendKeysToEmulator)
                    {
                        SpectrumKeyboard.SendKeyUp((VirtualKey)e.Key);
                        return;
                    }
                };
            }
        }
    }

    private DebugResponse Debug_BeforeExecuteInstruction(DebugState state)
    {
        if (_firstDebug)
        {
            AddDebuggerOutput($"<bold><color:white>Breaking at <color:darkgray>0x{state.Address.ToString("X4")}</color></color></bold>");
            Thread.Sleep(100); // ensure ordering
            _firstDebug = false;
        }

        AddDebuggerOutput($"<color:{(_jumped ? "green" : "darkgray")}>0x{state.Address.ToString("X4")}</color> <color:white>{state.Disassembly}</color>");
            
        _waitingForNextInstructionButton = true;
        while (_waitingForNextInstructionButton)
        {
            // allow all other UI events to process
            Thread.Sleep(1);
        }

        if (_jumped) _jumped = false;
        return DebugResponse.None;
    }

    private DebugResponse Debug_AfterExecuteInstruction(DebugState state)
    {
        _jumped = state.ProgramCounterWasModified;

        var registers = _vm.CPU.Registers;
        var flags = _vm.CPU.Flags;

        string changedRegisters = "";

        if (state.Instruction.TargetsByteRegister)
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
                        changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:darkgray>{previousValue:X2}H</color>-><color:green>{currentValue:X2}H</color></bold>|";
                        _previousByteRegisterValues[registerName] = previousValue;
                    }
                    else if (byteRegister == state.Instruction.Target.AsByteRegister())
                    {
                        changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:green>{previousValue:X2}H</color></bold>|";
                    }
                }
            }
        }
        else if (state.Instruction.TargetsWordRegister)
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
                            changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:darkgray>{previousValue:X4}H</color>-><color:green>{currentValue:X4}H</color></bold>|";
                        }
                    }
                    else if (wordRegister == state.Instruction.Target.AsWordRegister())
                    {
                        changedRegisters += $"<bold><color:gray>{registerName}</color>:<color:green>{previousValue:X4}H</color></bold>|";
                    }
                }
            }
        }

        if (_jumped)
        {
            changedRegisters += $"<bold><color:gray>PC</color>:<color:green>{registers.PC:X4}</color></bold>";
        }

        string flagStatus = "<bold>";
        flagStatus += markUp('S', flags.Sign);
        flagStatus += markUp('Z', flags.Zero);
        flagStatus += markUp('Y', flags.Y);
        flagStatus += markUp('H', flags.HalfCarry);
        flagStatus += markUp('X', flags.X);
        flagStatus += markUp('P', flags.ParityOverflow);
        flagStatus += markUp('N', flags.Subtract);
        flagStatus += markUp('C', flags.Carry);
        flagStatus += "</bold>";

        string markUp(char flag, bool state)
        {
            return state ? $"<color:green>{flag}</color>" : "<color:gray>_</color>";
        }

        Dispatcher.Dispatch(() =>
        {
            changedRegisters = changedRegisters.TrimEnd(',', '|');
            changedRegisters = flagStatus + " " + changedRegisters;
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

        return _debuggingStopped ? DebugResponse.Stop : DebugResponse.StepNext;
    }

    public DisplayPage(SettingsManager settingsManager)
    {
        InitializeComponent();

        // Get settings manager from DI via constructor injection
        _settingsManager = settingsManager;
        
        // Initialize debugger labels
        MAX_DEBUGGER_LINES = 100;
        _debuggerLabels = new Label[MAX_DEBUGGER_LINES];
        
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
        BreakpointAddressEntry.Focused += (s, e) => { _sendKeysToEmulator = false; };
        BreakpointAddressEntry.Unfocused += (s, e) => { _sendKeysToEmulator = true; };
        RunToAddressEntry.Focused += (s, e) => { _sendKeysToEmulator = false; };
        RunToAddressEntry.Unfocused += (s, e) => { _sendKeysToEmulator = true; };

        BreakpointAddressEntry.Completed += (s, e) => OnAddBreakpointClicked(s, e);
        RunToAddressEntry.Completed += (s, e) => OnRunToAddressClicked(s, e);

        // MAUI does not have Loaded, Focusable, Focused, Unfocused, KeyDown, KeyUp on ContentPage.
        // Use Appearing event and attach keyboard events to a view that can receive focus, e.g., SKCanvasView.
        this.Appearing += DisplayPage_Appearing;
    }
}
