using Harry.Data.DbLink;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Harry.Extensions.Logging.OrchardCore
{
    public class XYSoftLoggerDbLinkProvider : IDbLinkProvider
    {
        private readonly XYSoftLoggerOptions options;
        private readonly IServiceProvider sp;
        public XYSoftLoggerDbLinkProvider(IOptions<XYSoftLoggerOptions> optionsAccessor, IServiceProvider serviceProvider)
        {
            options = optionsAccessor.Value;
            this.sp = serviceProvider;
        }

        public DbLinkItem GetDbLink(string name)
        {
            if (!string.Equals(name, options.DbLinkName, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var shellSettings = sp.GetService<ShellSettings>();
            // Before the setup a 'DatabaseProvider' may be configured without a required 'ConnectionString'.
            if (shellSettings.State != TenantState.Running || shellSettings["DatabaseProvider"] == null)
            {
                return null;
            }

            var result = new DbLinkItem()
            {
                Name = options.DbLinkName,
                ConnectionString = shellSettings["ConnectionString"]
            };

            switch (shellSettings["DatabaseProvider"])
            {
                case "SqlConnection":
                    result.DbType = "SqlServer";
                    break;
                case "Sqlite":
                    var shellOptions = sp.GetService<IOptions<ShellOptions>>();
                    var option = shellOptions.Value;
                    var databaseFolder = Path.Combine(option.ShellsApplicationDataPath, option.ShellsContainerName, shellSettings.Name);
                    var databaseFile = Path.Combine(databaseFolder, "yessql.db");
                    if (!Directory.Exists(databaseFolder))
                    {
                        Directory.CreateDirectory(databaseFolder);
                    }
                    result.DbType = "Sqlite";
                    result.ConnectionString = $"Data Source={databaseFile};Cache=Shared";
                    break;
                case "MySql":
                    result.DbType = "MySql";
                    break;
                case "Postgres":
                    result.DbType = "Postgres";
                    break;
                default:
                    throw new ArgumentException("Unknown database provider: " + shellSettings["DatabaseProvider"]);
            }
            return result;
        }

    }
}
