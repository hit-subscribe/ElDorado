namespace ElDorado.Domain
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public partial class BlogContext : DbContext
    {
        public BlogContext() : base("name=BlogContext") { }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogMetric> BlogMetrics { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogMetrics)
                .WithRequired(b => b.Blog)
                .HasForeignKey(b => b.BlogId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogPosts)
                .WithRequired(b => b.Blog)
                .HasForeignKey(b => b.BlogId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Author>()
                .HasMany(b => b.BlogPosts)
                .WithOptional(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .WillCascadeOnDelete(true);
        }
    }
}
