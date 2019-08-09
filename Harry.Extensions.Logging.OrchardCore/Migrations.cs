using OrchardCore.Data.Migration;
using System;

namespace Harry.Extensions.Logging.OrchardCore
{
    public class Migrations : DataMigration
    {
        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>执行下一个对应的 UpdateFrom{版本号} 方法.创建成功返回大于或等于1的值</returns>
        public int Create()
        {
            //日志数据库
            SchemaBuilder.CreateTable("Logs", table => table
                .Column<long>("Id", col => col.PrimaryKey().Identity().NotNull())
                .Column<string>("CategoryName", c => c.NotNull().WithLength(100))
                .Column<int>("LogLevel")
                .Column<int>("EventId")
                .Column<string>("Message")
                .Column<int>("CreatorId")
                .Column<string>("CreatorName", c => c.WithLength(100))
                .Column<DateTime>("CreationTime")
            );

            //SchemaBuilder.AlterTable(nameof(DBLinkIndex), table => table
            //    .CreateIndex("IDX_ContentItemIndex_ContentItemId", "ContentItemId", "Latest", "Published", "CreatedUtc")
            //);

            //SchemaBuilder.AlterTable(nameof(DBLinkIndex), table => table
            //    .CreateIndex("IDX_ContentItemIndex_ContentItemVersionId", "ContentItemVersionId")
            //);

            return 1;
        }

        ///// <summary>
        ///// 更新表
        ///// </summary>
        ///// <returns>执行下一个对应的 UpdateFrom{版本号} 方法</returns>
        //public int UpdateFrom1()
        //{
        //    SchemaBuilder.AlterTable(nameof(DBLinkIndex), table => table
        //        .AddColumn<string>("ContentItemVersionId", c => c.WithLength(26))
        //    );

        //    SchemaBuilder.AlterTable(nameof(DBLinkIndex), table => table
        //        .CreateIndex("IDX_ContentItemIndex_ContentItemVersionId", "ContentItemVersionId")
        //    );

        //    return 2;
        //}

        ///// <summary>
        ///// 卸载/移除表
        ///// </summary>
        //public void Uninstall()
        //{

        //}

    }
}
