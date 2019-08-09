using System;
using System.Collections.Generic;
using System.Text;
using Harry.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Microsoft.Extensions.Options;

namespace Harry.Extensions.Logging.OrchardCore
{
    public class XYSoftLogger : ILogger
    {
        private readonly XYSoftLoggerProvider loggerProvider;
        private readonly XYSoftLoggerOptions options;
        public XYSoftLogger(XYSoftLoggerProvider loggerProvider, string categoryName)
        {
            this.loggerProvider = loggerProvider ?? throw new ArgumentNullException(nameof(loggerProvider));
            this.CategoryName = categoryName;

            options = loggerProvider.ServiceProvider.GetRequiredService<IOptions<XYSoftLoggerOptions>>().Value;
        }

        public string CategoryName { get; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }

            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (options.Filter != null && !options.Filter.Invoke(logLevel, eventId, this.CategoryName))
            {
                return;
            }

            string message = null;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else if (exception != null)
            {
                message = exception.ToString();
            }

            if (!string.IsNullOrEmpty(message))
            {
                WriteMessage(logLevel, eventId.Id, message);
            }
        }

        private void WriteMessage(LogLevel logLevel, int eventId, string message)
        {
            if (loggerProvider.Repository == null)
                return;

            var model = new LogModel()
            {
                CategoryName = this.CategoryName,
                LogLevel = (int)logLevel,
                EventId = eventId,
                Message = message,
            };

            //todo:这里应该放到队列里,然后再批量插入数据库
            loggerProvider.Repository.Insert(model);
        }
    }
}
