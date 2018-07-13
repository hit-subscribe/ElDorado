namespace ElDorado.Domain
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public partial class BlogContext : IdentityDbContext<AppUser>
    {
        public BlogContext() : base("name=BlogContext") { }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogMetric> BlogMetrics { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        public virtual void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
