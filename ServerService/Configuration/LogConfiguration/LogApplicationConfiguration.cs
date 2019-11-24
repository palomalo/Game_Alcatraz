using Serilog;
using Serilog.Events;
using System;

namespace ServerService.Configuration.LogConfiguration
{
    public static class LogApplicationConfiguration
    {
        public static void ConfigureLogging(ConfigurationManager configurationManager)
        {
            var logFile = string.Format(configurationManager.LogFile, DateTime.Today.ToShortDateString().Replace("/", string.Empty));

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug().MinimumLevel
                .Override("Microsoft", LogEventLevel.Information).Enrich
                .FromLogContext()
                .WriteTo.File($"{ConfigurationManager.ApplicationPath}/{configurationManager.LogFolder}/{logFile}");

            Serilog.Log.Logger = loggerConfiguration.CreateLogger(); ;
        }

        public static void ConfigureLog(this ConfigurationManager configurationManager)
        {
            ConfigureLogging(configurationManager);
        }
    }
}
