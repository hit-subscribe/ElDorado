namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingauthortothedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Bio = c.String(),
                        BlogUgrl = c.String(),
                        EmailAddress = c.String(),
                        TrelloId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BlogPosts", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogPosts", "AuthorId");
            AddForeignKey("dbo.BlogPosts", "AuthorId", "dbo.Authors", "Id", cascadeDelete: true);
            DropColumn("dbo.BlogPosts", "Author");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogPosts", "Author", c => c.String());
            DropForeignKey("dbo.BlogPosts", "AuthorId", "dbo.Authors");
            DropIndex("dbo.BlogPosts", new[] { "AuthorId" });
            DropColumn("dbo.BlogPosts", "AuthorId");
            DropTable("dbo.Authors");
        }
    }
}
