namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBlogEntity1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        CompanyName = c.String(),
                        FeedlyUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Blogs");
        }
    }
}
