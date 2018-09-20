namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDraftCompleteData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "DraftCompleteDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "DraftCompleteDate");
        }
    }
}
