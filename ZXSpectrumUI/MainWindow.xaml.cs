using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Z80.ZXSpectrumVM;

namespace Z80.ZXSpectrumUI
{
    public class MainWindow : Window
    {
        private Z80.ZXSpectrumVM.Spectrum48K _vm;
        private int _drawsSinceLastFlash;
        private WriteableBitmap _bitmap;
        private DispatcherTimer _timer;

        public Image Display { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _vm = new Spectrum48K();
            _vm.Start();
            Display.Source = _bitmap;
            _vm.OnUpdateDisplay += UpdateDisplay;
        }

        private void UpdateDisplay(object sender, ScreenBuffer buffer)
        {
            if (_bitmap == null) return;

            byte[] pixels = new byte[256 * 192 * 4];
            int pixelIndex = 0;
            for (int row = 0; row < 192; row++)
            {
                for (int column = 0; column < 256; column++)
                {
                    for (int rgbIndex = 0; rgbIndex < 4; rgbIndex++)
                    {
                        pixels[pixelIndex++] = getPixel(row, column, rgbIndex, _drawsSinceLastFlash);
                    }
                }
            }

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var bitmap = _bitmap.Lock();
                Marshal.Copy(pixels, 0, bitmap.Address, pixels.Length);
                Display.InvalidateVisual();
            });

            _drawsSinceLastFlash++;
            if (_drawsSinceLastFlash > 50)
            {
                _drawsSinceLastFlash = 0;
            }

            byte getPixel(int row, int column, int rgbIndex, int drawsSinceLastFlash)
            {
                if (rgbIndex == 3) return 0xFF; // alpha channel is always max
                //return (byte)(new Random().Next(0, 255));

                (bool onOff, DisplayAttribute attribute) pixel = (buffer.Pixels[row, column], buffer.Attributes[(row / 8), (column / 8)]);
                (byte, byte, byte) ink = pixel.attribute.Bright ? pixel.attribute.Ink.Bright : pixel.attribute.Ink.Normal;
                (byte, byte, byte) paper = pixel.attribute.Bright ? pixel.attribute.Paper.Bright : pixel.attribute.Paper.Normal;
                (byte R, byte G, byte B) pixelColour;
                if (pixel.attribute.Flash && drawsSinceLastFlash < 26)
                {
                    pixelColour = ink;
                    ink = paper;
                    paper = pixelColour;
                }
                pixelColour = pixel.onOff ? ink : paper;

                return rgbIndex switch { 0 => pixelColour.R, 1 => pixelColour.G, 2 => pixelColour.B, _ => 0x00 };
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Display = this.FindControl<Image>("Display");
            _bitmap = new WriteableBitmap(new PixelSize(256, 192), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
        }
    }
}
