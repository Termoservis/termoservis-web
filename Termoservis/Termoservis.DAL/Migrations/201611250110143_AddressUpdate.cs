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
            this.DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            this.DropIndex("dbo.Addresses", new[] { "PlaceId" });
            this.AlterColumn("dbo.Addresses", "PlaceId", c => c.Int());
            this.CreateIndex("dbo.Addresses", "PlaceId");
            this.AddForeignKey("dbo.Addresses", "PlaceId", "dbo.Places", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            this.DropIndex("dbo.Addresses", new[] { "PlaceId" });
            this.AlterColumn("dbo.Addresses", "PlaceId", c => c.Int(nullable: false));
            this.CreateIndex("dbo.Addresses", "PlaceId");
            this.AddForeignKey("dbo.Addresses", "PlaceId", "dbo.Places", "Id", cascadeDelete: true);
        }
    }
}
