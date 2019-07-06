using System;
using System.Collections.Generic;
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
    }
}
