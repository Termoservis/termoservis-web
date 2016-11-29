namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDeviceInCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices");
            DropIndex("dbo.WorkItems", new[] { "DeviceId" });
            AddColumn("dbo.CustomerDevices", "Customer_Id", c => c.Long());
            CreateIndex("dbo.CustomerDevices", "Customer_Id");
            AddForeignKey("dbo.CustomerDevices", "Customer_Id", "dbo.Customers", "Id");
            DropColumn("dbo.WorkItems", "DeviceId");
        }
        
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
