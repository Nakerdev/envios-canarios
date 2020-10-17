using System;
using NLog;

namespace CanaryDeliveries.WebApp.Api.Logs
{
    public sealed class NLogLogger : Logger
    {
        private readonly string fullNameClass;
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public static Logger Create(Type classType) 
        {
            return new NLogLogger(classType.FullName);
        }

        private NLogLogger(string fullNameClass)
        {
            this.fullNameClass = fullNameClass;
        }

        public void LogError(Exception exception, string messageTemplate)
        {
            Log(LogLevel.Error, messageTemplate, fullNameClass, exception);
        }

        private static void Log(LogLevel logLevel, string message, string loggerName, Exception ex)
        {
            if (!Logger.IsEnabled(logLevel)) return;

            var eventInfo = new LogEventInfo
            {
                Message = message,
                Level = logLevel,
                LoggerName = loggerName,
                Exception = ex,
            };
            eventInfo.Properties.Add("FullStackTrace", ex.ToString());

            Logger.Log(eventInfo);
        }
    }
}