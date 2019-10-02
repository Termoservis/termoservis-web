using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Adds the customer device and work items models to the database.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class WorkItem : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            this.CreateTable(
                "dbo.CustomerDevices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.WorkItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Price = c.Int(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        DeviceId = c.Long(),
                        Customer_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerDevices", t => t.DeviceId)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.DeviceId)
                .Index(t => t.Customer_Id);
            
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.WorkItems", "Customer_Id", "dbo.Customers");
            this.DropForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices");
            this.DropIndex("dbo.WorkItems", new[] { "Customer_Id" });
            this.DropIndex("dbo.WorkItems", new[] { "DeviceId" });
            this.DropTable("dbo.WorkItems");
            this.DropTable("dbo.CustomerDevices");
        }
    }
}
