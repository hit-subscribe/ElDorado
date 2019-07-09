namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRefresh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogPosts", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.BlogPosts", "EditorId", "dbo.Editors");
            CreateTable(
                "dbo.PostRefreshes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogPostId = c.Int(nullable: false),
                        AuthorId = c.Int(),
                        DraftDate = c.DateTime(),
                        SubmittedDate = c.DateTime(),
                        TargetPublicationDate = c.DateTime(),
                        Published = c.DateTime(),
                        TrelloId = c.String(),
                        AuthorPay = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPostId)
                .ForeignKey("dbo.Authors", t => t.AuthorId)
                .Index(t => t.BlogPostId)
                .Index(t => t.AuthorId);
            
            AddForeignKey("dbo.BlogPosts", "AuthorId", "dbo.Authors", "Id");
            AddForeignKey("dbo.BlogPosts", "EditorId", "dbo.Editors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "EditorId", "dbo.Editors");
            DropForeignKey("dbo.BlogPosts", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.PostRefreshes", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.PostRefreshes", "BlogPostId", "dbo.BlogPosts");
            DropIndex("dbo.PostRefreshes", new[] { "AuthorId" });
            DropIndex("dbo.PostRefreshes", new[] { "BlogPostId" });
            DropTable("dbo.PostRefreshes");
            AddForeignKey("dbo.BlogPosts", "EditorId", "dbo.Editors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BlogPosts", "AuthorId", "dbo.Authors", "Id", cascadeDelete: true);
        }
    }
}
