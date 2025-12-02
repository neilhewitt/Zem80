namespace ZXSpectrum_MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell(DisplayPage displayPage)
        {
            InitializeComponent();
            
            // Set the display page instance from DI
            var shellContent = Items[0].CurrentItem.CurrentItem as ShellContent;
            if (shellContent != null)
            {
                shellContent.ContentTemplate = new DataTemplate(() => displayPage);
            }
        }
    }
}
