using ElDorado.Domain;
using ElDorado.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Repository
{
    public class BlogPostRepository
    {
        private readonly BlogContext _context;
        public BlogPostRepository(BlogContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        public void Add(IEnumerable<BlogPost> posts)
        {
            foreach (var post in posts)
                AssignRelationshipsToPost(post);

            _context.BlogPosts.AddRange(posts);
            _context.SaveChanges();
        }

        private void AssignRelationshipsToPost(BlogPost post)
        {
            post.Blog = _context.Blogs.FirstOrDefault(b => b.CompanyName == post.BlogCompanyName);
            if (post.Blog == null)
                throw new OrphanedBlogPostException();
            post.Author = _context.Authors.FirstOrDefault(a => a.FirstName == post.PostAuthorFirstName);
        }
    }
}
