namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingInOurSystems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "IsInOurSystems", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "IsInOurSystems");
        }
    }
}
