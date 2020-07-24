using ElDorado.Console.Trello;
using ElDorado.Domain;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class PostRefreshesController : CrudTrelloController<PostRefresh>
    {
        public PostRefreshesController(BlogContext context, ITrelloSynchronizer<PostRefresh> synchronizer) : base(context, synchronizer)
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

            var refresh = new PostRefresh() { BlogPostId = blogPostId };
            return View(refresh);
        }

        public override ViewResult Edit(int id)
        {
            ViewBag.Authors = Context.Authors;
            return base.Edit(id);
        }

        public override ViewResult Edit(PostRefresh refresh)
        {
            ViewBag.Authors = Context.Authors;
            return base.Edit(refresh);
        }

        public override ActionResult Delete(int id)
        {
            var postId = Context.PostRefreshes.First(pr => pr.Id == id).BlogPostId;
            
            base.Delete(id);
            
            return RedirectToAction("Index", new { blogPostId = postId });
        }
    }
}