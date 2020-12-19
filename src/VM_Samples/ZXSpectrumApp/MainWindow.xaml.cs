using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXEm.VM;
using Zem80.Core;

namespace ZXEm.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static object _updateLock = new object();

        private Spectrum48K _vm;
        private bool _isClosing = false;
        private bool _showCode = false;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new Spectrum48K("rom\\48k.rom");
            _vm.OnUpdateDisplay += UpdateDisplay;
            _vm.Start();
            _vm.CPU.Debug.AfterExecute += AfterInstructionExecution;

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
        }

        private void AfterInstructionExecution(object sender, ExecutionResult e)
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
                    if (Code.Items.Count >= 25) Code.Items.RemoveAt(0);
                    Code.Items.Add(mnemonic);
                });
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            SpectrumKeyboard.KeyUp(e.Key.ToString());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            SpectrumKeyboard.KeyDown(e.Key.ToString());
        }

        private void RunTests(object sender, RoutedEventArgs e)
        {
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
