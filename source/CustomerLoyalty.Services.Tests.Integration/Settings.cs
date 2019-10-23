using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public static class Settings
    {
        public static T GetFromAppSettings<T>(string buildName = null) where T : class, new()
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
