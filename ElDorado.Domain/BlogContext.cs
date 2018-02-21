namespace ElDorado.Domain
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public partial class BlogContext : DbContext
    {
        public BlogContext()
            : base("name=BlogContext")
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogMetric> BlogMetrics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogMetrics)
                .WithRequired(b => b.Blog)
                .HasForeignKey(b => b.BlogId)
                .WillCascadeOnDelete(true);
        }
    }
}
