using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.ViewModels
{
    public class BlogPostIndexViewModel : BlogPostViewModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; private set; }

        public BlogPostIndexViewModel(IEnumerable<BlogPost> posts, BlogContext context) : base(context)
        {
            BlogPosts = posts;
        }
    }
}