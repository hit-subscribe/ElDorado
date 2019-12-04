namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingDefault : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPosts", "QualifiesForAuthorBonus", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogPosts", "QualifiesForAuthorBonus", c => c.Boolean(nullable: false));
        }
    }
}
