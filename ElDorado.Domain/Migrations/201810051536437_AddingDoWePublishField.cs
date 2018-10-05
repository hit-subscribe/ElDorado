namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDoWePublishField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "DoWePublish", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "DoWePublish");
        }
    }
}
