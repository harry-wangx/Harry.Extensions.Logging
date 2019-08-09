using Harry.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Models;
using System;
using System.Collections.Concurrent;

namespace Harry.Extensions.Logging.OrchardCore
{
    /// <summary>
    /// LoggerProvider,负责生成Logger
    /// </summary>
    public class XYSoftLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, XYSoftLogger> _loggers = new ConcurrentDictionary<string, XYSoftLogger>();
        private IRepository repository;
        private readonly XYSoftLoggerOptions options;
        private readonly IServiceScope serviceScope;
        private readonly IRepositoryFactory repositoryFactory;

        private readonly object locker = new object();
        public XYSoftLoggerProvider(IServiceProvider sp, IOptions<XYSoftLoggerOptions> optionsAccessor)
        {
            this.options = optionsAccessor.Value;
            this.serviceScope = sp.CreateScope();
            repositoryFactory = new RepositoryFactory(serviceScope.ServiceProvider);
        }

        public IRepository Repository
        {
            get
            {
                if (repository != null)
                {
                    return repository;
                }

                var shellSettings = serviceScope.ServiceProvider.GetService<ShellSettings>();
                if (shellSettings.State != TenantState.Running || shellSettings["DatabaseProvider"] == null)
                {
                    return null;
                }

                lock (locker)
                {
                    if (repository != null)
                    {
                        return repository;
                    }

                    repository = repositoryFactory.CreateRepository<LogModel>(options.DbLinkName);
                    return repository;
                }
            }
        }

        public IServiceProvider ServiceProvider { get { return serviceScope.ServiceProvider; } }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        private XYSoftLogger CreateLoggerImplementation(string categoryName)
        {
            return new XYSoftLogger(this, categoryName);
        }

        public void Dispose()
        {
            serviceScope.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
