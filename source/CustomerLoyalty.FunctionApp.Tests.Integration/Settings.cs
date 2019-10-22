using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Tests.Integration
{
    public static class Settings
    {
        public static T GetFromAppsettings<T>(string buildName = null) where T : class, new()
        {
            T settings = new T();

            buildName = buildName == null ? "" : "." + buildName;

            string settingsFile = $"appsettings{buildName}.json";

            settingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), settingsFile);

            new ConfigurationBuilder()
                .AddJsonFile(settingsFile)
                .Build()
                .Bind(settings.GetType().Name, settings);

            return settings;
        }
    }
}
