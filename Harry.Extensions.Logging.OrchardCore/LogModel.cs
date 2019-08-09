using Harry.Data.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions.Logging.OrchardCore
{
    public class LogModel : Harry.Data.Entities.Entity<long>, ICreationAudited<int>
    {
        public string CategoryName { get; set; }

        public int LogLevel { get; set; }

        public int EventId { get; set; }

        public string Message { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreationTime { get; set; }

    }
}
