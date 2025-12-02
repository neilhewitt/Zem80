namespace ZXSpectrum_MAUI.Settings
{
    /// <summary>
    /// Application settings loaded from appsettings.json
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Show debugger pane on startup (default: false)
        /// </summary>
        public bool DebuggerAvailable { get; set; } = false;

        /// <summary>
        /// Path to ROM file (leave empty to use default)
        /// </summary>
        public string RomPath { get; set; } = "";

        /// <summary>
        /// Show file picker on startup if no snapshot is specified (default: false)
        /// </summary>
        public bool ShowFilePickerOnStartup { get; set; } = false;

        /// <summary>
        /// Mute audio when debugger is visible (default: true)
        /// </summary>
        public bool MuteWhenDebugging { get; set; } = true;
    }
}
