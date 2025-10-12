using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.ComponentModel;
using ZXSpectrum.VM;
using Microsoft.Maui.Controls; // Add this for ContentPage, KeyEventArgs, etc.

namespace ZXSpectrum_MAUI;

public partial class DisplayPage : ContentPage
{
    private Spectrum48K _vm;
    private bool _isClosing = false;
    private SKBitmap _currentBitmap;
    private readonly object _bitmapLock = new object();

    public DisplayPage()
    {
        InitializeComponent();

        // MAUI does not have Loaded, Focusable, Focused, Unfocused, KeyDown, KeyUp on ContentPage.
        // Use Appearing event and attach keyboard events to a view that can receive focus, e.g., SKCanvasView.
        this.Appearing += DisplayPage_Appearing;
    }

    private async void DisplayPage_Appearing(object sender, EventArgs e)
    {
        _vm = new Spectrum48K(UpdateDisplay);

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

#if WINDOWS
        SetupWindowsKeyboardHandling();
#endif
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
                    SpectrumKeyboard.MauiKeyDown((int)e.Key);
                };

                content.KeyUp += (s, e) =>
                {
                    SpectrumKeyboard.MauiKeyUp((int)e.Key);
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

}
