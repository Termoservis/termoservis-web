namespace Termoservis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Worker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WorkItems", "WorkerId", c => c.Long());
            CreateIndex("dbo.WorkItems", "WorkerId");
            AddForeignKey("dbo.WorkItems", "WorkerId", "dbo.Workers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkItems", "WorkerId", "dbo.Workers");
            DropIndex("dbo.WorkItems", new[] { "WorkerId" });
            DropColumn("dbo.WorkItems", "WorkerId");
            DropTable("dbo.Workers");
        }
    }
}
