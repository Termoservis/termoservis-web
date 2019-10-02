using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Adds the address, place, country, customer and telephone number models to the database.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class Customer : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            this.CreateTable(
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

            this.CreateTable(
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

            this.CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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

            this.CreateTable(
                "dbo.TelephoneNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        SearchKeywords = c.String(nullable: false),
                        Customer_Id = c.Long(),
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
            this.DropForeignKey("dbo.TelephoneNumbers", "Customer_Id", "dbo.Customers");
            this.DropForeignKey("dbo.Customers", "ApplicationUserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Customers", "AddressId", "dbo.Addresses");
            this.DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            this.DropForeignKey("dbo.Places", "CountryId", "dbo.Countries");
            this.DropIndex("dbo.TelephoneNumbers", new[] { "Customer_Id" });
            this.DropIndex("dbo.Customers", new[] { "ApplicationUserId" });
            this.DropIndex("dbo.Customers", new[] { "AddressId" });
            this.DropIndex("dbo.Places", new[] { "CountryId" });
            this.DropIndex("dbo.Addresses", new[] { "PlaceId" });
            this.DropTable("dbo.TelephoneNumbers");
            this.DropTable("dbo.Customers");
            this.DropTable("dbo.Countries");
            this.DropTable("dbo.Places");
            this.DropTable("dbo.Addresses");
        }
    }
}
