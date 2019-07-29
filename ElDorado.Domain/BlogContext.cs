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
        public virtual DbSet<Editor> Editors { get; set; }
        public virtual DbSet<PostRefresh> PostRefreshes { get; set; }
        public virtual DbSet<Whitepaper> Whitepapers { get; set; }
        public virtual void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogMetrics)
                .WithRequired(bm => bm.Blog)
                .HasForeignKey(bm => bm.BlogId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogPosts)
                .WithRequired(bp => bp.Blog)
                .HasForeignKey(bp => bp.BlogId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Whitepapers)
                .WithRequired(wp => wp.Blog)
                .HasForeignKey(wp => wp.BlogId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.BlogPosts)
                .WithOptional(bp => bp.Author)
                .HasForeignKey(bp => bp.AuthorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Whitepapers)
                .WithOptional(wp => wp.Author)
                .HasForeignKey(wp => wp.AuthorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.PostRefreshes)
                .WithOptional(pr => pr.Author)
                .HasForeignKey(pr => pr.AuthorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Editor>()
                .HasMany(e => e.BlogPosts)
                .WithOptional(bp => bp.Editor)
                .HasForeignKey(bp => bp.EditorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Editor>()
                .HasMany(e => e.Whitepapers)
                .WithOptional(wp => wp.Editor)
                .HasForeignKey(wp => wp.EditorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogPost>()
                .HasMany(bp => bp.PostRefreshes)
                .WithRequired(pr => pr.BlogPost)
                .HasForeignKey(pr => pr.BlogPostId)
                .WillCascadeOnDelete(false);
        }
    }
}
