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
        private CodeWindow _code;
        private bool _displaySource = true;
        private BindingList<string> _asm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new Spectrum48K();
            _vm.OnUpdateDisplay += UpdateDisplay;
            //_vm.OnAfterExecuteInstruction += _vm_OnAfterExecuteInstruction;

            //_code = new CodeWindow();
           // _code.Show();

            //_asm = new BindingList<string>();
            //_code.AssemblyListing.ItemsSource = _asm;
            
            _vm.Start();
        }

        private void _vm_OnAfterExecuteInstruction(object sender, (string mnemonic, string status) e)
        {
            lock (_updateLock)
            {
                _code.Dispatcher.Invoke(() =>
                {
                    _asm.Add(e.mnemonic);
                    if (_asm.Count > 20) _asm.RemoveAt(0);
                });
                Thread.Sleep(0);
            }
        }

        public void UpdateDisplay(object sender, byte[] rgba)
        {
            lock (_updateLock)
            {
                Dispatcher.Invoke(() =>
                {
                    var bitmap = BitmapFactory.New(256, 192).FromByteArray(rgba);
                    DisplaySurface.Source = bitmap;
                });
            }
        }

        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            _displaySource = !_displaySource;
        }
    }
}
