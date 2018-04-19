namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Makingauthorforeignkeynullableforblogpost : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BlogPosts", new[] { "AuthorId" });
            AlterColumn("dbo.BlogPosts", "AuthorId", c => c.Int());
            CreateIndex("dbo.BlogPosts", "AuthorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlogPosts", new[] { "AuthorId" });
            AlterColumn("dbo.BlogPosts", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogPosts", "AuthorId");
        }
    }
}
