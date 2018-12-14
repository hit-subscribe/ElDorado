using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class CrudController<T> : Controller where T : class, IHaveIdentity, new()
    {
        private readonly BlogContext _context;
        protected BlogContext Context => _context;

        protected Func<IEnumerable<T>, IEnumerable<T>> IndexSortFunction { get; set; } = entities => entities.OrderBy(e => e.Id);

        public CrudController(BlogContext context)
        {
            _context = context;
        }

        public virtual ActionResult Index()
        {
            return View(IndexSortFunction(_context.Set<T>()));
        }
        public virtual ViewResult Create()
        {
            return View(new T());
        }

        [HttpPost]
        public virtual ActionResult Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = entity.Id });
        }
        public virtual ViewResult Edit(int id)
        {
            return View(_context.Set<T>().First(e => e.Id == id));
        }

        [HttpPost]
        public ViewResult Edit(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.SetModified(entity);
            _context.SaveChanges();
            return View(_context.Set<T>().First(e => e.Id == entity.Id));
        }
        public ActionResult Delete(int id)
        {
            var entityToDelete = _context.Set<T>().First(e => e.Id == id);
            _context.Set<T>().Remove(entityToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}