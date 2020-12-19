using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Timers;
using ZXEm.VM;

namespace ZXEm.UI
{
    public class MainWindow : Window
    {
        private static object _updateLock = new object();

        private Spectrum48K _vm;
        private bool _isClosing = false;
        private bool _sendingKeys = false;
        private List<string> _keysDown = new List<string>();
        private int _frameCount;
        private byte[] _screenBuffer;
        private bool _drawingScreen;
        private Image _displaySurface;
        private WriteableBitmap _displayBuffer;
 
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _vm = new Spectrum48K("48k.rom");
            _vm.OnUpdateDisplay += UpdateDisplay;
            _vm.Start(GetKeysDown);

            _displaySurface = this.FindControl<Image>("DisplaySurface");
            //_displayBuffer = new WriteableBitmap(new PixelSize(320, 256), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;

            Timer timer = new Timer(200);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _drawingScreen = true;

            if (!_isClosing)
            {
                lock (_updateLock)
                {
                    if (!_isClosing)
                    {
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            _displayBuffer = new WriteableBitmap(new PixelSize(320, 256), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
                            using (var buffer = _displayBuffer.Lock())
                            {
                                Marshal.Copy(_screenBuffer, 0, buffer.Address, _screenBuffer.Length);
                            }

                            _displaySurface.Source = _displayBuffer;
                        });
                    }
                }

                _frameCount = 0;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private string[] GetKeysDown()
        {
            _sendingKeys = true;
            string[] keysDown = _keysDown.ToArray();
            _sendingKeys = false;
            return keysDown;
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (!_sendingKeys)
            {
                _keysDown.Remove(e.Key.ToString());
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_sendingKeys && !_keysDown.Contains(e.Key.ToString()))
            {
                _keysDown.Add(e.Key.ToString());
            }
        }

        public void UpdateDisplay(object sender, byte[] rgba)
        {
            while (_drawingScreen) ;
            _screenBuffer = rgba;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _isClosing = true;
            _vm.Stop();
            base.OnClosing(e);
        }
    }
}
