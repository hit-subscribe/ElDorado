using ElDorado.Domain;
using ElDorado.Trello;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class CrudTrelloController<T> : CrudController<T> where T : class, IHaveIdentity, IHaveTrelloId, new()
    {
        protected ITrelloSynchronizer<T> Synchronizer { get; private set; }

        public string MapPath { get; set; }

        public CrudTrelloController(BlogContext context, ITrelloSynchronizer<T> synchronizer) : base(context)
        {
            Synchronizer = synchronizer;
        }

        public override ActionResult Create(T entity)
        {
            var result = base.Create(entity);

            var reloadedEntity = Context.Reload(entity);

            Synchronizer.Initialize(MapPath ?? Server.MapPath(@"~/App_Data/trello.cred"));
            Synchronizer.CreateCardForEntity(reloadedEntity);

            Context.SaveChanges();

            return result;
        }

        public override ViewResult Edit(T entity)
        {
            var result = base.Edit(entity);

            var reloadedEntity = Context.Reload(entity);

            Synchronizer.Initialize(MapPath ?? Server.MapPath(@"~/App_Data/trello.cred"));
            Synchronizer.UpdateCardForEntity(reloadedEntity);

            return result;
        }

        public override ActionResult Delete(int id)
        {
            var trelloIdToDelete = Context.Set<T>().First(e => e.Id == id).TrelloId;
            var result = base.Delete(id);

            Synchronizer.Initialize(MapPath ?? Server.MapPath(@"~/App_Data/trello.cred"));
            Synchronizer.DeleteCard(trelloIdToDelete);

            return result;
        }
    }
}