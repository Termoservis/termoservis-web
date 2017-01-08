using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Added work items and customer devices to the database.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class CustomerDeviceInCustomer : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            DropForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices");
            DropIndex("dbo.WorkItems", new[] { "DeviceId" });
            AddColumn("dbo.CustomerDevices", "Customer_Id", c => c.Long());
            CreateIndex("dbo.CustomerDevices", "Customer_Id");
            AddForeignKey("dbo.CustomerDevices", "Customer_Id", "dbo.Customers", "Id");
            DropColumn("dbo.WorkItems", "DeviceId");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AddColumn("dbo.WorkItems", "DeviceId", c => c.Long());
            DropForeignKey("dbo.CustomerDevices", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.CustomerDevices", new[] { "Customer_Id" });
            DropColumn("dbo.CustomerDevices", "Customer_Id");
            CreateIndex("dbo.WorkItems", "DeviceId");
            AddForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices", "Id");
        }
    }
}
