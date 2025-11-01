using System.IO;
using System.Text.Json;

namespace InventoryApp.Services
{
    public class SettingsService
    {
        private readonly string _settingsPath;

        public SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "InventoryApp");
            Directory.CreateDirectory(appFolder);
            _settingsPath = Path.Combine(appFolder, "settings.json");
        }

        public UserSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch
            {
                // If there's any error reading the file, return default settings
            }
            
            return new UserSettings();
        }

        public void SaveSettings(UserSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_settingsPath, json);
            }
            catch
            {
                // Silently handle save errors
            }
        }
    }

    public class UserSettings
    {
        public bool RememberMe { get; set; }
        public string SavedEmail { get; set; } = string.Empty;
        public string SavedPassword { get; set; } = string.Empty; // This will be encrypted
    }
}