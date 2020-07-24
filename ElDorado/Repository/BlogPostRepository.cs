using ElDorado.Console.Exceptions;
using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.Console.Repository
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
            var postsToAdd = posts.Where(p => p.Id == 0).ToList();

            foreach (var post in postsToAdd)
                AssignRelationshipsToPost(post);

            _context.BlogPosts.AddRange(postsToAdd);
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
