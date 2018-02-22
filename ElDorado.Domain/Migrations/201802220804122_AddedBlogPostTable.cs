namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBlogPostTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        UrlSlug = c.String(),
                        DraftDate = c.DateTime(nullable: false),
                        TargetFinalizeDate = c.DateTime(nullable: false),
                        TargetPublicationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "BlogId", "dbo.Blogs");
            DropIndex("dbo.BlogPosts", new[] { "BlogId" });
            DropTable("dbo.BlogPosts");
        }
    }
}
