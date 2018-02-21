namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBlogMetricEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogMetrics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogId = c.Int(nullable: false),
                        Recorded = c.DateTime(nullable: false),
                        FeedlySubscribers = c.Int(nullable: false),
                        AlexaRanking = c.Int(nullable: false),
                        DomainAuthority = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkingRootDomains = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogMetrics", "BlogId", "dbo.Blogs");
            DropIndex("dbo.BlogMetrics", new[] { "BlogId" });
            DropTable("dbo.BlogMetrics");
        }
    }
}
