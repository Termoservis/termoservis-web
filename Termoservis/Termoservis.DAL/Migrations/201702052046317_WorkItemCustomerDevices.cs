using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Added CustomerDevices to WorkItem model.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class WorkItemCustomerDevices : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.CustomerDevices", "WorkItem_Id", c => c.Long());
            CreateIndex("dbo.CustomerDevices", "WorkItem_Id");
            AddForeignKey("dbo.CustomerDevices", "WorkItem_Id", "dbo.WorkItems", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.CustomerDevices", "WorkItem_Id", "dbo.WorkItems");
            DropIndex("dbo.CustomerDevices", new[] { "WorkItem_Id" });
            DropColumn("dbo.CustomerDevices", "WorkItem_Id");
        }
    }
}
