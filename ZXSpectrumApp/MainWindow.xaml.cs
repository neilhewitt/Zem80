using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Z80.Core;
using Z80.ZXSpectrumVM;

namespace Z80.ZXSpectrumApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 69888
        private static object _updateLock = new object();

        private Spectrum48K _vm;
        private bool _displaySource = true;
        private bool _isClosing = false;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new Spectrum48K();
            _vm.OnUpdateDisplay += UpdateDisplay;
            _vm.Start();
        }

        public void UpdateDisplay(object sender, byte[] rgba)
        {
            if (!_isClosing)
            {
                lock (_updateLock)
                {
                    if (!_isClosing)
                    {
                        Dispatcher.InvokeAsync(() =>
                        {
                            var bitmap = BitmapFactory.New(320, 256).FromByteArray(rgba);
                            DisplaySurface.Stretch = Stretch.Fill;
                            DisplaySurface.Source = bitmap;
                        });
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _isClosing = true;
            _vm.Stop();
            base.OnClosing(e);
        }

        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            _displaySource = !_displaySource;
        }
    }
}
