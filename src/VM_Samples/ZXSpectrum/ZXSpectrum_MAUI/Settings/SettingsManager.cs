using System.Text.Json;

namespace ZXSpectrum_MAUI.Settings
{
    /// <summary>
    /// Service for loading and managing application settings
    /// </summary>
    public class SettingsManager
    {
        private const string SETTINGS_FILENAME = "appsettings.json";
        private AppSettings _settings;

        public AppSettings Settings => _settings;

        public SettingsManager()
        {
            _settings = LoadSettings();
        }

        /// <summary>
        /// Load settings from appsettings.json file
        /// </summary>
        private AppSettings LoadSettings()
        {
            try
            {
                // Try to load from the app's directory first
                string settingsPath = Path.Combine(AppContext.BaseDirectory, SETTINGS_FILENAME);
                
                if (!File.Exists(settingsPath))
                {
                    // Fall back to embedded resource or create default settings
                    return CreateDefaultSettings();
                }

                string json = File.ReadAllText(settingsPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var settings = JsonSerializer.Deserialize<AppSettings>(json, options);
                return settings ?? CreateDefaultSettings();
            }
            catch (Exception ex)
            {
                // Log error and return default settings
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
                return CreateDefaultSettings();
            }
        }

        /// <summary>
        /// Create default settings
        /// </summary>
        private AppSettings CreateDefaultSettings()
        {
            return new AppSettings();
        }

        /// <summary>
        /// Save current settings to appsettings.json
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                string settingsPath = Path.Combine(AppContext.BaseDirectory, SETTINGS_FILENAME);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string json = JsonSerializer.Serialize(_settings, options);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Update settings and optionally save to file
        /// </summary>
        public void UpdateSettings(Action<AppSettings> updateAction, bool save = false)
        {
            updateAction(_settings);
            if (save)
            {
                SaveSettings();
            }
        }

        /// <summary>
        /// Reset settings to defaults
        /// </summary>
        public void ResetToDefaults(bool save = false)
        {
            _settings = CreateDefaultSettings();
            if (save)
            {
                SaveSettings();
            }
        }
    }
}
