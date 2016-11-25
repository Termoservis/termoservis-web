namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDevice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerDevices", "Manufacturer", c => c.String());
            AddColumn("dbo.CustomerDevices", "CommissionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerDevices", "CommissionDate");
            DropColumn("dbo.CustomerDevices", "Manufacturer");
        }
    }
}
