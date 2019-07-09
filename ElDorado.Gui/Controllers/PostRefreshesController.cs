using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class PostRefreshesController : CrudController<PostRefresh>
    {
        public PostRefreshesController(BlogContext context) : base(context)
        {
            IndexSortFunction = refreshes => refreshes.OrderBy(r => r.DraftDate);
        }

        public override ActionResult Index(int blogPostId = 0)
        {
            var matchingRefreshes = Context.PostRefreshes.Where(pr => pr.BlogPostId == blogPostId || blogPostId == 0);
            return View("Index", IndexSortFunction(matchingRefreshes));
        }
        public override ViewResult Create(int blogPostId = 0)
        {
            ViewBag.Authors = Context.Authors;
            return View(new PostRefresh() { BlogPostId = blogPostId });
        }

        public override ViewResult Edit(int id)
        {
            ViewBag.Authors = Context.Authors;
            return base.Edit(id);
        }
    }
}