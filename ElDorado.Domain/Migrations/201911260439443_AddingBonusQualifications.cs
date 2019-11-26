namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBonusQualifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "QualifiesForAuthorBonus", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "StaffNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "StaffNotes");
            DropColumn("dbo.BlogPosts", "QualifiesForAuthorBonus");
        }
    }
}
