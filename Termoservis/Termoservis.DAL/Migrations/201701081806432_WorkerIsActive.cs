using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Added IsActive to the worker.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class WorkerIsActive : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Workers", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.Workers", "IsActive");
        }
    }
}
