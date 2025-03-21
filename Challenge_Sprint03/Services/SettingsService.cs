using Challenge_Sprint03.Models;

namespace Challenge_Sprint03.Services
{
    public class SettingsService
    {
        private readonly AppSettings _appSettings;

        // Singleton: A instância é compartilhada por toda a aplicação
        private static SettingsService _instance;

        private SettingsService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public static SettingsService GetInstance(AppSettings appSettings)
        {
            if (_instance == null)
            {
                _instance = new SettingsService(appSettings);
            }
            return _instance;
        }

        public string GetConfigValue()
        {
            return _appSettings.ConfigValue;
        }
    }
}
