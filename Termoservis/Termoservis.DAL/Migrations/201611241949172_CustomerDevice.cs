using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Adds the manufacturer and comission date to the customer device.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class CustomerDevice : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            this.AddColumn("dbo.CustomerDevices", "Manufacturer", c => c.String());
            this.AddColumn("dbo.CustomerDevices", "CommissionDate", c => c.DateTime());
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.DropColumn("dbo.CustomerDevices", "CommissionDate");
            this.DropColumn("dbo.CustomerDevices", "Manufacturer");
        }
    }
}
