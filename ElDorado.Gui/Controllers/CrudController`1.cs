using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class CrudController<T> : Controller where T : class, IHaveIdentity, new()
    {
        protected BlogContext Context { get; private set; }

        protected Func<IEnumerable<T>, IEnumerable<T>> IndexSortFunction { get; set; } = entities => entities.OrderBy(e => e.Id);

        public CrudController(BlogContext context)
        {
            Context = context;
        }

        public virtual ActionResult Index()
        {
            return View(IndexSortFunction(Context.Set<T>()));
        }
        public virtual ViewResult Create()
        {
            return View(new T());
        }

        [HttpPost]
        public virtual ActionResult Create(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();

            return RedirectToAction("Edit", new { id = entity.Id });
        }
        public virtual ViewResult Edit(int id)
        {
            return View(Context.Set<T>().First(e => e.Id == id));
        }

        [HttpPost]
        public ViewResult Edit(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.SetModified(entity);
            Context.SaveChanges();
            return View(Context.Set<T>().First(e => e.Id == entity.Id));
        }
        public ActionResult Delete(int id)
        {
            var entityToDelete = Context.Set<T>().First(e => e.Id == id);
            Context.Set<T>().Remove(entityToDelete);
            Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}