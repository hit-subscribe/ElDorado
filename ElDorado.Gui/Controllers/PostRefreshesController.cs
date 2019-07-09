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
        public ActionResult Index(int blogPostId)
        {
            var matchingRefreshes = Context.PostRefreshes.Where(pr => pr.BlogPostId == blogPostId);
            return View(IndexSortFunction(matchingRefreshes));
        }
    }
}