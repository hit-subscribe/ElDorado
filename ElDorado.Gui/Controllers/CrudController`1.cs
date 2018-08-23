using ElDorado.Domain;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class CrudController<T> : Controller where T : class, IHaveIdentity, new()
    {
        private readonly BlogContext _context;

        public CrudController(BlogContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.Set<T>());
        }
        public ViewResult Create()
        {
            return View(new T());
        }

        [HttpPost]
        public ActionResult Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = entity.Id });
        }
        public ViewResult Edit(int id)
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