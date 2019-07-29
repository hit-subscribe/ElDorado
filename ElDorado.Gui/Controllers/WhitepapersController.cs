using ElDorado.Domain;
using ElDorado.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class WhitepapersController : CrudTrelloController<Whitepaper>
    {
        public WhitepapersController(BlogContext context, ITrelloSynchronizer<Whitepaper> synchronizer) : base(context, synchronizer)
        {

        }

        public override ViewResult Create(int id = 0)
        {
            SetupViewbag();

            return base.Create(id);
        }

        public override ViewResult Edit(int id)
        {
            SetupViewbag();

            return base.Edit(id);
        }

        public override ViewResult Edit(Whitepaper entity)
        {
            SetupViewbag();

            return base.Edit(entity);
        }

        private void SetupViewbag()
        {
            ViewBag.Authors = Context.Authors.Where(a => a.IsActive);
            ViewBag.Blogs = Context.Blogs.Where(b => b.IsActive);
            ViewBag.Editors = Context.Editors.Where(e => e.IsActive);
        }
    }
}