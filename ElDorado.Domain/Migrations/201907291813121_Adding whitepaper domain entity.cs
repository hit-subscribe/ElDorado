namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingwhitepaperdomainentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Whitepapers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogId = c.Int(nullable: false),
                        AuthorId = c.Int(),
                        EditorId = c.Int(),
                        Title = c.String(),
                        Mission = c.String(),
                        Persona = c.String(),
                        Notes = c.String(),
                        TargetOutlineDate = c.DateTime(),
                        OutlineSubmittedDate = c.DateTime(),
                        TargetDraftDate = c.DateTime(),
                        DraftSubmittedDate = c.DateTime(),
                        TargeSubmissionDate = c.DateTime(),
                        SubmittedDate = c.DateTime(),
                        IsGhostwritten = c.Boolean(nullable: false),
                        TrelloId = c.String(),
                        AuthorPay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EditorPay = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Editors", t => t.EditorId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.AuthorId)
                .Index(t => t.BlogId)
                .Index(t => t.AuthorId)
                .Index(t => t.EditorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Whitepapers", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.Whitepapers", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Whitepapers", "EditorId", "dbo.Editors");
            DropIndex("dbo.Whitepapers", new[] { "EditorId" });
            DropIndex("dbo.Whitepapers", new[] { "AuthorId" });
            DropIndex("dbo.Whitepapers", new[] { "BlogId" });
            DropTable("dbo.Whitepapers");
        }
    }
}
