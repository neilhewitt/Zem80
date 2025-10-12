using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXSpectrum.VM;

namespace ZXSpectrum.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Spectrum48K _vm;
        private bool _isClosing = false;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new Spectrum48K(UpdateDisplay);

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;

            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string imagePath = dialog.FileName;
                _vm.Start(imagePath);
            }
            else
            {
                _vm.Start();
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            SpectrumKeyboard.WpfKeyUp((int)e.Key);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            SpectrumKeyboard.WpfKeyDown((int)e.Key);
        }

        private void RunTests(object sender, RoutedEventArgs e)
        {
        }

        public void UpdateDisplay(byte[] rgba)
        {
            if (!_isClosing)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    var bitmap = BitmapFactory.New(320, 256).FromByteArray(rgba);
                    DisplaySurface.Stretch = Stretch.Uniform;
                    DisplaySurface.Source = bitmap;
                });
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _isClosing = true;
            _vm?.Stop();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }
    }
}
