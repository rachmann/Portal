using System.Configuration;
namespace Portal.Common
{
    public static class AppSettings
    {
        public static string GetSettingOrDefault(string setting, string defaultValue)
        {
            if (string.IsNullOrEmpty(setting))
            {
                return defaultValue;
            }
            else
            {
                return setting;
            }
        }

    }

    public static class ConnectionStrings
    {
        public static readonly ConnectionStringSettings ActiveDbConnection = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ActiveDBConnection"] ?? "DefaultConnection"];
    }

}