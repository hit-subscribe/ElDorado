namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingWordpressPropertiesToBlogPosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "Content", c => c.String());
            AddColumn("dbo.BlogPosts", "WordpressId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "WordpressId");
            DropColumn("dbo.BlogPosts", "Content");
        }
    }
}
