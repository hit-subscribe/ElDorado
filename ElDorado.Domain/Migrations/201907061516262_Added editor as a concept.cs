namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addededitorasaconcept : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Editors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailAddress = c.String(),
                        TrelloId = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsInOurSystems = c.Boolean(nullable: false),
                        BaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WordpressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BlogPosts", "EditorId", c => c.Int());
            AddColumn("dbo.BlogPosts", "EditorPay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.BlogPosts", "EditorId");
            AddForeignKey("dbo.BlogPosts", "EditorId", "dbo.Editors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "EditorId", "dbo.Editors");
            DropIndex("dbo.BlogPosts", new[] { "EditorId" });
            DropColumn("dbo.BlogPosts", "EditorPay");
            DropColumn("dbo.BlogPosts", "EditorId");
            DropTable("dbo.Editors");
        }
    }
}
