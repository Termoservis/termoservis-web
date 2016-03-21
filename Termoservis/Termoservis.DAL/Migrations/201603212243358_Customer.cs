namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

	/// <summary>
	/// Migration that adds Customer model and corresponding models.
	/// </summary>
	/// <seealso cref="System.Data.Entity.Migrations.DbMigration" />
	/// <seealso cref="System.Data.Entity.Migrations.Infrastructure.IMigrationMetadata" />
	public partial class Customer : DbMigration
    {
		/// <summary>
		/// Operations to be performed during the upgrade process.
		/// </summary>
		public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreetAddress = c.String(nullable: false),
                        PlaceId = c.Int(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .Index(t => t.PlaceId);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Note = c.String(),
                        Email = c.String(),
                        AddressId = c.Int(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.AddressId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.TelephoneNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }

		/// <summary>
		/// Operations to be performed during the downgrade process.
		/// </summary>
		public override void Down()
        {
            DropForeignKey("dbo.TelephoneNumbers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Places", "CountryId", "dbo.Countries");
            DropIndex("dbo.TelephoneNumbers", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Customers", new[] { "AddressId" });
            DropIndex("dbo.Places", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "PlaceId" });
            DropTable("dbo.TelephoneNumbers");
            DropTable("dbo.Customers");
            DropTable("dbo.Countries");
            DropTable("dbo.Places");
            DropTable("dbo.Addresses");
        }
    }
}
