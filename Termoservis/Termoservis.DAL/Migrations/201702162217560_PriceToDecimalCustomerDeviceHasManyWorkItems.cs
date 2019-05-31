using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// The price to decimal and customer device has many work items fix.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class PriceToDecimalCustomerDeviceHasManyWorkItems : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            this.DropForeignKey("dbo.CustomerDevices", "WorkItem_Id", "dbo.WorkItems");
            this.DropIndex("dbo.CustomerDevices", new[] { "WorkItem_Id" });
            this.CreateTable(
                "dbo.WorkItemCustomerDevices",
                c => new
                    {
                        WorkItem_Id = c.Long(nullable: false),
                        CustomerDevice_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkItem_Id, t.CustomerDevice_Id })
                .ForeignKey("dbo.WorkItems", t => t.WorkItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerDevices", t => t.CustomerDevice_Id, cascadeDelete: true)
                .Index(t => t.WorkItem_Id)
                .Index(t => t.CustomerDevice_Id);

            this.AlterColumn("dbo.WorkItems", "Price", c => c.Double(nullable: false));
            this.DropColumn("dbo.CustomerDevices", "WorkItem_Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.AddColumn("dbo.CustomerDevices", "WorkItem_Id", c => c.Long());
            this.DropForeignKey("dbo.WorkItemCustomerDevices", "CustomerDevice_Id", "dbo.CustomerDevices");
            this.DropForeignKey("dbo.WorkItemCustomerDevices", "WorkItem_Id", "dbo.WorkItems");
            this.DropIndex("dbo.WorkItemCustomerDevices", new[] { "CustomerDevice_Id" });
            this.DropIndex("dbo.WorkItemCustomerDevices", new[] { "WorkItem_Id" });
            this.AlterColumn("dbo.WorkItems", "Price", c => c.Int(nullable: false));
            this.DropTable("dbo.WorkItemCustomerDevices");
            this.CreateIndex("dbo.CustomerDevices", "WorkItem_Id");
            this.AddForeignKey("dbo.CustomerDevices", "WorkItem_Id", "dbo.WorkItems", "Id");
        }
    }
}
