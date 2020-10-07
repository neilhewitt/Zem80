using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Zem80.ZXSpectrumVM;

namespace Zem80.ZXSpectrumApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static object _updateLock = new object();
        private static object _keyLock = new object();

        private Spectrum48K _vm;
        private bool _isClosing = false;
        private bool _showCode = false;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new Spectrum48K();
            _vm.OnUpdateDisplay += UpdateDisplay;
            _vm.Start();
            _vm.CPU.Debug.AfterExecute += AfterInstructionExecution;

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
        }

        private void AfterInstructionExecution(object sender, Core.ExecutionResult e)
        {
            if (_showCode)
            {
                string mnemonic = e.InstructionAddress.ToString("X4") + ": " + e.Instruction.Mnemonic;
                if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", e.Data.ArgumentsAsWord.ToString("X4"));
                if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", e.Data.Argument1.ToString("X2"));
                if (mnemonic.Contains("n") && !mnemonic.Contains("o")) mnemonic = mnemonic.Replace("n", e.Data.Argument1.ToString("X2"));
                if (mnemonic.Contains("n") && mnemonic.Contains("o")) mnemonic = mnemonic.Replace("n", e.Data.Argument2.ToString("X2"));

                Dispatcher.Invoke(() =>
                {
                    if (Code.Items.Count >= 30) Code.Items.RemoveAt(0);
                    Code.Items.Add(mnemonic);
                });
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.KeyUp(e.Key.ToString());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _vm.KeyDown(e.Key.ToString());
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
            Application.Current.Shutdown();
        }

        private void ToggleCode(object sender, RoutedEventArgs e)
        {
            _showCode = !_showCode;
        }
    }
}
