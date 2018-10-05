namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPublishedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "PublishedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "PublishedDate");
        }
    }
}
