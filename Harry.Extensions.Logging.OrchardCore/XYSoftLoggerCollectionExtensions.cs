using Harry.Data;
using Harry.Data.DbLink;
using Harry.Extensions.Logging.OrchardCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Data.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class XYSoftLoggerCollectionExtensions
    {
        public static IServiceCollection AddOrchardCoreLoggerProvider(this IServiceCollection services, Action<XYSoftLoggerOptions> optionsBuilder = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, XYSoftLoggerProvider>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDbLinkProvider, XYSoftLoggerDbLinkProvider>());
            //数据数据迁移类
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IDataMigration, Migrations>());

            services.AddSqlite();
            services.AddSqlServer();
            services.AddPomeloMySql();

            services.AddEFCoreRepository(options =>
            {
                options.Register<LogModel>(e =>
                {
                    e.ToTable("Logs");
                    e.HasKey(m => m.Id);
                });
            });

            if (optionsBuilder != null)
            {
                services.Configure(optionsBuilder);
            }

            var descriptor = services.Where(m => m.ServiceType == typeof(ILoggerFactory)).FirstOrDefault();

            if (descriptor != null)
            {
                services.Remove(descriptor);
                services.AddLogging();
            }

            return services;
        }
    }
}
