using Exceptionless;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions.Logging.Exceptionless
{
    public static class ExceptionlessLoggerExtensions
    {
        //public static ILoggingBuilder AddConsole(this ILoggingBuilder builder)
        //{
        //    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
        //    return builder;
        //}


        public static IServiceCollection TryAddXYExceptionless(this IServiceCollection services, ExceptionlessClient client = null)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(new ExceptionlessLoggerProvider(client ?? ExceptionlessClient.Default)));
            return services;
        }
        public static IServiceCollection TryAddXYExceptionless(this IServiceCollection services, string apiKey, string serverUrl = null)
        {
            if (String.IsNullOrEmpty(apiKey) && String.IsNullOrEmpty(serverUrl))
                return services.TryAddXYExceptionless();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(new ExceptionlessLoggerProvider(config =>
            {
                if (!String.IsNullOrEmpty(apiKey) && apiKey != "API_KEY_HERE")
                    config.ApiKey = apiKey;
                if (!String.IsNullOrEmpty(serverUrl))
                    config.ServerUrl = serverUrl;

                config.UseInMemoryStorage();
            }

                )));
            return services;
        }


        public static IServiceCollection TryAddXYExceptionless(this IServiceCollection services, Action<ExceptionlessConfiguration> configure)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(new ExceptionlessLoggerProvider(configure)));
            return services;
        }


        public static ILoggingBuilder TryAddXYExceptionless(this ILoggingBuilder builder, ExceptionlessClient client = null)
        {
            builder.Services.TryAddXYExceptionless(client);

            return builder;
        }


        public static ILoggingBuilder TryAddXYExceptionless(this ILoggingBuilder builder, string apiKey, string serverUrl = null)
        {
            builder.Services.TryAddXYExceptionless(apiKey, serverUrl);
            return builder;
        }


        public static ILoggingBuilder TryAddXYExceptionless(this ILoggingBuilder builder, Action<ExceptionlessConfiguration> configure)
        {
            builder.Services.TryAddXYExceptionless(configure);
            return builder;
        }
    }
}
