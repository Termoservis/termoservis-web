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
            this.DropForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices");
            this.DropIndex("dbo.WorkItems", new[] { "DeviceId" });
            this.AddColumn("dbo.CustomerDevices", "Customer_Id", c => c.Long());
            this.CreateIndex("dbo.CustomerDevices", "Customer_Id");
            this.AddForeignKey("dbo.CustomerDevices", "Customer_Id", "dbo.Customers", "Id");
            this.DropColumn("dbo.WorkItems", "DeviceId");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.AddColumn("dbo.WorkItems", "DeviceId", c => c.Long());
            this.DropForeignKey("dbo.CustomerDevices", "Customer_Id", "dbo.Customers");
            this.DropIndex("dbo.CustomerDevices", new[] { "Customer_Id" });
            this.DropColumn("dbo.CustomerDevices", "Customer_Id");
            this.CreateIndex("dbo.WorkItems", "DeviceId");
            this.AddForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices", "Id");
        }
    }
}
