namespace ZXSpectrum_MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new AppShell())
            {
                Title = "ZX Spectrum Emulator",
                Width = 640,
                Height = 552
            };
        }
    }
}