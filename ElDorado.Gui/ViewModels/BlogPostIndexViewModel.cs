using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.ViewModels
{
    public class BlogPostIndexViewModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; private set; }

        public IEnumerable<SelectListItem> Blogs { get; private set; } = Enumerable.Empty<SelectListItem>();

        public BlogPostIndexViewModel(IEnumerable<BlogPost> posts, BlogContext context)
        {
            BlogPosts = posts;
            Blogs = context.Blogs.Select(b => new SelectListItem() { Text = b.CompanyName, Value = b.Id.ToString() });
        }
    }
}