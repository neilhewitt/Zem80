using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Z80.ZXSpectrumApp
{
    /// <summary>
    /// Interaction logic for CodeWindow.xaml
    /// </summary>
    public partial class CodeWindow : Window
    {
        public CodeWindow()
        {
            InitializeComponent();
        }

        public void AddOutput(string output)
        {
            ListViewItem item = new ListViewItem()
            {
                Content = output
            };
            if (AssemblyListing.Items.Count > 50) AssemblyListing.Items.Clear();
            AssemblyListing.Items.Add(item);
            AssemblyListing.ScrollIntoView(AssemblyListing.Items[AssemblyListing.Items.Count - 1]);
        }
    }
}
