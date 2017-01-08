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
            DropForeignKey("dbo.WorkItems", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.WorkItems", new[] { "Customer_Id" });
            RenameColumn(table: "dbo.WorkItems", name: "Customer_Id", newName: "CustomerId");
            AlterColumn("dbo.WorkItems", "CustomerId", c => c.Long(nullable: false));
            CreateIndex("dbo.WorkItems", "CustomerId");
            AddForeignKey("dbo.WorkItems", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.WorkItems", "CustomerId", "dbo.Customers");
            DropIndex("dbo.WorkItems", new[] { "CustomerId" });
            AlterColumn("dbo.WorkItems", "CustomerId", c => c.Long());
            RenameColumn(table: "dbo.WorkItems", name: "CustomerId", newName: "Customer_Id");
            CreateIndex("dbo.WorkItems", "Customer_Id");
            AddForeignKey("dbo.WorkItems", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
