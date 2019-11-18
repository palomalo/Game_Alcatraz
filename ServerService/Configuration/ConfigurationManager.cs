using Microsoft.Extensions.Configuration;
using ServerService.Configuration.LogConfiguration;
using System;
using System.IO;
using System.Linq;

namespace ServerService.Configuration
{
    public class ConfigurationManager
    {
        private IConfiguration Configuration { get; set; }
        public string LogFile { get; set; }
        public string LogFolder { get; set; }
        public string ApplicationName { get; set; }
        public static string ApplicationPath { get; set; }

        public ConfigurationManager()
        {
            DiscoverApplicationPath();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(ApplicationPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables().Build();

            Configure();
        }

        public IConfiguration GetConfiguration()
        {
            return Configuration;
        }

        private string GetConfigurationValue(string key)
        {
            return Configuration[key];
        }

        private void Configure()
        {
            LogFile = GetConfigurationValue("Log:File:LogFile");
            LogFolder = GetConfigurationValue("Log:File:LogFolder");
            if (string.IsNullOrEmpty(LogFolder)) LogFolder = "Logs";
            if (string.IsNullOrEmpty(LogFile)) LogFile = "log-{0}.txt";
            ServerOptions.IPAddress = GetConfigurationValue("ServerConfig:Address");
            ServerOptions.Port = Convert.ToInt32(GetConfigurationValue("ServerConfig:Port"));

            this.ConfigureLog();
        }
        public static void DiscoverApplicationPath()
        {
            ApplicationPath = Path.GetDirectoryName(Directory.GetFiles(Directory.GetCurrentDirectory(), "appsettings.json", SearchOption.AllDirectories).FirstOrDefault());
        }
    }
}