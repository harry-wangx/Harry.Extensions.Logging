using Microsoft.Extensions.Logging;
using System;

namespace Harry.Extensions.Logging.OrchardCore
{
    public class XYSoftLoggerOptions
    {
        private string dbLinkName = "_XYSoft_OrchardCore_Logs";
        /// <summary>
        /// 数据库链接名称
        /// </summary>
        public string DbLinkName
        {
            get { return this.dbLinkName; }
            //set
            //{
            //    if (string.IsNullOrEmpty(value))
            //    {
            //        throw new NullReferenceException("设置DbLinkName时出错,值不能为空");
            //    }
            //    dbLinkName = value;
            //}
        }

        /// <summary>
        /// 日志过滤器
        /// </summary>
        public Func<LogLevel, EventId, string, bool> Filter { get; set; } = (logLevel, eventId, categoryName) => eventId.Id >= 10000 && eventId.Id < 20000;
    }
}
