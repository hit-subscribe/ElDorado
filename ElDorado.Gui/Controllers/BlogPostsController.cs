using ElDorado.Domain;
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
    }
}