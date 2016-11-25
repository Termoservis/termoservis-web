namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerDevices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
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
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkItems", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.WorkItems", "DeviceId", "dbo.CustomerDevices");
            DropIndex("dbo.WorkItems", new[] { "Customer_Id" });
            DropIndex("dbo.WorkItems", new[] { "DeviceId" });
            DropTable("dbo.WorkItems");
            DropTable("dbo.CustomerDevices");
        }
    }
}