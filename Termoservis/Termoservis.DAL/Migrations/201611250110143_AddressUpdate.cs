using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Updates addresses model so that it includes place as not required.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class AddressUpdate : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            DropIndex("dbo.Addresses", new[] { "PlaceId" });
            AlterColumn("dbo.Addresses", "PlaceId", c => c.Int());
            CreateIndex("dbo.Addresses", "PlaceId");
            AddForeignKey("dbo.Addresses", "PlaceId", "dbo.Places", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
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
