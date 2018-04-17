namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingKeywordtoBlogPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "Keyword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "Keyword");
        }
    }
}
