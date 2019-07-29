using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public partial class BlogContext
    {
        public virtual void UpdateBlogPostDependencies(BlogPost post)
        {
            Entry(post).Reference(p => p.Author).Load();
            Entry(post).Reference(p => p.Blog).Load();
            Entry(post).Reference(p => p.Editor).Load();
        }

        public virtual void UpdateRefreshDependencies(PostRefresh refresh)
        {
            Entry(refresh).Reference(pr => pr.Author).Load();
            Entry(refresh).Reference(pr => pr.BlogPost).Load();
            Entry(refresh.BlogPost).Reference(bp => bp.Blog).Load();
        }

        public virtual T SaveAndReload<T>(T entity) where T : class, IHaveIdentity
        {
            SaveChanges();
            return Reload(entity);
        }

        public virtual T Reload<T>(T entity) where T : class, IHaveIdentity
        {
            Entry(entity).State = EntityState.Detached;
            return Set<T>().First(e => e.Id == entity.Id);
        }
    }
}
