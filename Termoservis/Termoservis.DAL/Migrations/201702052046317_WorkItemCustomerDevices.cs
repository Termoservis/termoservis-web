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
            this.AddColumn("dbo.CustomerDevices", "WorkItem_Id", c => c.Long());
            this.CreateIndex("dbo.CustomerDevices", "WorkItem_Id");
            this.AddForeignKey("dbo.CustomerDevices", "WorkItem_Id", "dbo.WorkItems", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.CustomerDevices", "WorkItem_Id", "dbo.WorkItems");
            this.DropIndex("dbo.CustomerDevices", new[] { "WorkItem_Id" });
            this.DropColumn("dbo.CustomerDevices", "WorkItem_Id");
        }
    }
}
