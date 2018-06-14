using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.ViewModels
{
    public class BlogPostViewModel
    {

        public BlogPost Post { get; private set; }

        public IEnumerable<SelectListItem> Blogs { get; private set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Authors { get; private set; } = Enumerable.Empty<SelectListItem>();

        public BlogPostViewModel() : this(new BlogPost(), null)
        {

        }

        public BlogPostViewModel(BlogPost post, BlogContext context)
        {
            Post = post;
            if (context != null)
            {
                Blogs = context.Blogs.Select(b => new SelectListItem() { Text = b.CompanyName, Value = b.Id.ToString() });
                Authors = context.Authors.Select(a => new SelectListItem() { Text = a.FirstName, Value = a.Id.ToString() });
            }
        }
    }
}