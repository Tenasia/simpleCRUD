using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace OnePhp.HRIS.Core
{
    public class Config
    {
        public readonly string _connectionString = string.Empty;
        public readonly string _sentryConnectionString = String.Empty;
        public readonly string _externalUrl = string.Empty;
        public readonly string _employeeURL = string.Empty;
        public Config()
        {
            var configurationBuilder = new ConfigurationBuilder();
#if DEBUG
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.development.json");
#else
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
#endif

            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            _sentryConnectionString = root.GetSection("ConnectionStrings").GetSection("TAMS").Value;
            var appSetting = root.GetSection("ApplicationSettings");

            _externalUrl = root.GetSection("External").GetSection("Url").Value;
            _employeeURL = root.GetSection("Welcome").GetSection("Url").Value;
        }

        public string ConnectionString
        {
            get => _connectionString;
        }

        public string SentryConnectionString
        {
            get => _sentryConnectionString;
        }

        public string ExternalUrl
        {
            get => _externalUrl;
        }
        public string EmployeeUrl
        {
            get => _employeeURL;
        }
    }
}
