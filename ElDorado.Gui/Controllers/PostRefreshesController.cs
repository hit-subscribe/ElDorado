using ElDorado.Domain;
using ElDorado.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class PostRefreshesController : CrudController<PostRefresh>
    {
        private readonly RefreshCalendarService _refreshService;

        public string MapPath { get; set; }

        public PostRefreshesController(BlogContext context, RefreshCalendarService refreshService) : base(context)
        {
            _refreshService = refreshService;
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

        public override ActionResult Create(PostRefresh refresh)
        {
            Context.PostRefreshes.Add(refresh);
            Context.SaveChanges();
            Context.UpdateRefreshDependencies(refresh);

            InitializeTrelloService();
            _refreshService.AddCard(refresh);

            return RedirectToAction("Edit", new { id = refresh.Id });
        }

        public override ViewResult Edit(int id)
        {
            ViewBag.Authors = Context.Authors;
            return base.Edit(id);
        }

        private void InitializeTrelloService()
        {
            _refreshService.Initialize(MapPath ?? Server.MapPath(@"~/App_Data/trello.cred"));
        }
    }
}