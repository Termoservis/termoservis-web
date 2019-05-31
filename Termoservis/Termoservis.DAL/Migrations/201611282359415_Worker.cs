using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Adds the worker model to the database.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class Worker : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            this.CreateTable(
                "dbo.Workers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            this.AddColumn("dbo.WorkItems", "WorkerId", c => c.Long());
            this.CreateIndex("dbo.WorkItems", "WorkerId");
            this.AddForeignKey("dbo.WorkItems", "WorkerId", "dbo.Workers", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.WorkItems", "WorkerId", "dbo.Workers");
            this.DropIndex("dbo.WorkItems", new[] { "WorkerId" });
            this.DropColumn("dbo.WorkItems", "WorkerId");
            this.DropTable("dbo.Workers");
        }
    }
}
