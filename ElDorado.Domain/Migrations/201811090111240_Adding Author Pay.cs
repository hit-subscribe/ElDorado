namespace ElDorado.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAuthorPay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "BaseRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BlogPosts", "AuthorPay", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "AuthorPay");
            DropColumn("dbo.Authors", "BaseRate");
        }
    }
}
