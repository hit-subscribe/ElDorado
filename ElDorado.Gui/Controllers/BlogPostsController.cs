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
        public ActionResult Index()
        {
            var context = new BlogContext();
            return View(context.BlogPosts);
        }
    }
}