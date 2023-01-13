using System;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace MealsOrderAPI.Logger {
    /// <summary>
    /// Create LogSettingOption for logging with default log level
    /// </summary>
    public class LogSettingOption {

        public LoggerProviderCollection Providers { get; }
        public LogEventLevel ApiMinimumLevel { get; }
        public LogEventLevel MicrosoftMinimumLevel { get; }

        /// <summary>
        /// Constructor for LogSettingOption
        /// </summary>
        /// <param name="apiMinimumLevel">Specify default log level for API (`Information` if not assigned)</param>
        /// <param name="microsoftMinimumLevel">Specify default log level for Microsoft library (`Warning` if not assigned)</param>
        public LogSettingOption(string apiMinimumLevel = "Information", string microsoftMinimumLevel = "Warning") {
            Providers = new LoggerProviderCollection();

            ApiMinimumLevel = LogEventLevelParse(apiMinimumLevel);
            MicrosoftMinimumLevel = LogEventLevelParse(microsoftMinimumLevel);
        }

        /// <summary>
        /// Transform input string to LogLevel property
        /// </summary>
        /// <param name="level">Specify string of log level</param>
        /// <returns>Specific LogEventLevel</returns>
        /// <exception cref="Exception"></exception>
        private static LogEventLevel LogEventLevelParse(string level) {
            return level switch {
                //"Verbose" => LogEventLevel.Verbose,
                "Debug" => LogEventLevel.Debug,
                "Information" => LogEventLevel.Information,
                "Warning" => LogEventLevel.Warning,
                "Error" => LogEventLevel.Error,
                //"Fatal" => LogEventLevel.Fatal,
                _ => throw new Exception($"{level} is not {nameof(LogEventLevel)}.")
            };
        }
    }

    /// <summary>
    /// Define logger's settings
    /// </summary>
    public class LogSetting {
        /// <summary>
        /// Setting logger's default log levels, output targets
        /// </summary>
        /// <param name="logSettingOption">Specify settings for logger</param>
        /// <returns>LoggerProviders for logging</returns>
        public static LoggerProviderCollection SetLog(LogSettingOption logSettingOption) {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new LogFormat(), logSettingOption.ApiMinimumLevel)
                .MinimumLevel.Is(logSettingOption.ApiMinimumLevel)
                .MinimumLevel.Override("Microsoft", logSettingOption.MicrosoftMinimumLevel)
                .WriteTo.Providers(logSettingOption.Providers)
                .CreateLogger();

            return logSettingOption.Providers;
        }
    }
}