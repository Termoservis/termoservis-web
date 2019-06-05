using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Termoservis.DAL.Migrations
{
    /// <summary>
    /// Added WorkItem Customer navigation property.
    /// </summary>
    /// <seealso cref="DbMigration" />
    /// <seealso cref="IMigrationMetadata" />
    public partial class WorkItemCustomerNavigationProperty : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            this.DropForeignKey("dbo.WorkItems", "Customer_Id", "dbo.Customers");
            this.DropIndex("dbo.WorkItems", new[] { "Customer_Id" });
            this.RenameColumn(table: "dbo.WorkItems", name: "Customer_Id", newName: "CustomerId");
            this.AlterColumn("dbo.WorkItems", "CustomerId", c => c.Long(nullable: false));
            this.CreateIndex("dbo.WorkItems", "CustomerId");
            this.AddForeignKey("dbo.WorkItems", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.WorkItems", "CustomerId", "dbo.Customers");
            this.DropIndex("dbo.WorkItems", new[] { "CustomerId" });
            this.AlterColumn("dbo.WorkItems", "CustomerId", c => c.Long());
            this.RenameColumn(table: "dbo.WorkItems", name: "CustomerId", newName: "Customer_Id");
            this.CreateIndex("dbo.WorkItems", "Customer_Id");
            this.AddForeignKey("dbo.WorkItems", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
