using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogTrace(this ILogger logger, Func<string> messageAction)
        {
            if (!logger.IsEnabled(LogLevel.Trace))
                return;

            logger.LogTrace(messageAction.Invoke());
        }
        public static void LogDebug(this ILogger logger, Func<string> messageAction)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
                return;

            logger.LogDebug(messageAction.Invoke());
        }

        public static void LogInformation(this ILogger logger, Func<string> messageAction)
        {
            if (!logger.IsEnabled(LogLevel.Information))
                return;

            logger.LogInformation(messageAction.Invoke());
        }

        public static void LogWarning(this ILogger logger, Func<string> messageAction)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
                return;

            logger.LogWarning(messageAction.Invoke());
        }

        public static void LogError(this ILogger logger, Func<string> messageAction)
        {
            if (!logger.IsEnabled(LogLevel.Error))
                return;

            logger.LogError(messageAction.Invoke());
        }

        public static void LogCritical(this ILogger logger, Func<string> messageAction)
        {
            if (!logger.IsEnabled(LogLevel.Critical))
                return;

            logger.LogCritical(messageAction.Invoke());
        }
    }
}
