namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTrelloId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "TrellId", c => c.String());
            DropColumn("dbo.BlogPosts", "TrelloId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogPosts", "TrelloId", c => c.String());
            DropColumn("dbo.BlogPosts", "TrellId");
        }
    }
}
