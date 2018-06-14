using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly BlogContext _blogContext;

        public BlogPostsController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public ActionResult Index()
        {
            return View(_blogContext.BlogPosts);
        }
        public ActionResult Edit(int id)
        {
            var blogPost = _blogContext.BlogPosts.First(bp => bp.Id == id);

            return View(new BlogPostViewModel(blogPost, _blogContext));
        }
    }
}