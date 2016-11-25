namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            DropIndex("dbo.Addresses", new[] { "PlaceId" });
            AlterColumn("dbo.Addresses", "PlaceId", c => c.Int());
            CreateIndex("dbo.Addresses", "PlaceId");
            AddForeignKey("dbo.Addresses", "PlaceId", "dbo.Places", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            DropIndex("dbo.Addresses", new[] { "PlaceId" });
            AlterColumn("dbo.Addresses", "PlaceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Addresses", "PlaceId");
            AddForeignKey("dbo.Addresses", "PlaceId", "dbo.Places", "Id", cascadeDelete: true);
        }
    }
}
