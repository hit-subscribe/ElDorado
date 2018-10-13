namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HandfulOfAdditions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "IsGhostwritten", c => c.Boolean(nullable: false));
            AddColumn("dbo.Blogs", "ClientPostNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "ClientPostNotes");
            DropColumn("dbo.BlogPosts", "IsGhostwritten");
            DropColumn("dbo.Authors", "IsActive");
        }
    }
}
