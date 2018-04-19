namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixingaspellingmistakeinanauthorcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "BlogUrl", c => c.String());
            DropColumn("dbo.Authors", "BlogUgrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Authors", "BlogUgrl", c => c.String());
            DropColumn("dbo.Authors", "BlogUrl");
        }
    }
}
