namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2XAndApproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "IsDoublePost", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "IsDoublePost");
            DropColumn("dbo.BlogPosts", "IsApproved");
        }
    }
}
