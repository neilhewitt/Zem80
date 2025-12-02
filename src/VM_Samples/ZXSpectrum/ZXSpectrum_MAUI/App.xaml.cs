namespace ZXSpectrum_MAUI
{
    public partial class App : Application
    {
        private readonly AppShell _appShell;

        public App(AppShell appShell)
        {
            InitializeComponent();
            _appShell = appShell;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(_appShell)
            {
                Title = "ZX Spectrum Emulator",
                Width = 640,
                Height = 552
            };
        }
    }
}