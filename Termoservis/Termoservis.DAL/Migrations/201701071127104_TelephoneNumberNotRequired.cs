namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TelephoneNumberNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TelephoneNumbers", "Number", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TelephoneNumbers", "Number", c => c.String(nullable: false));
        }
    }
}
