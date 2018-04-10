namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FleshingOutBlogPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "SeoTitle", c => c.String());
            AddColumn("dbo.BlogPosts", "Mission", c => c.String());
            AddColumn("dbo.BlogPosts", "SubmittedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BlogPosts", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "Author");
            DropColumn("dbo.BlogPosts", "SubmittedDate");
            DropColumn("dbo.BlogPosts", "Mission");
            DropColumn("dbo.BlogPosts", "SeoTitle");
        }
    }
}
