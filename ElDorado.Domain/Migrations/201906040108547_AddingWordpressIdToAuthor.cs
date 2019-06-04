namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingWordpressIdToAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "WordpressId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "WordpressId");
        }
    }
}
