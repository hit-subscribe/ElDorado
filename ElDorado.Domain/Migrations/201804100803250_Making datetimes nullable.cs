namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Makingdatetimesnullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPosts", "DraftDate", c => c.DateTime());
            AlterColumn("dbo.BlogPosts", "TargetFinalizeDate", c => c.DateTime());
            AlterColumn("dbo.BlogPosts", "TargetPublicationDate", c => c.DateTime());
            AlterColumn("dbo.BlogPosts", "SubmittedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogPosts", "SubmittedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BlogPosts", "TargetPublicationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BlogPosts", "TargetFinalizeDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BlogPosts", "DraftDate", c => c.DateTime(nullable: false));
        }
    }
}
