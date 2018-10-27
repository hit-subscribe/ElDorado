namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPostNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "PostNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "PostNotes");
        }
    }
}
