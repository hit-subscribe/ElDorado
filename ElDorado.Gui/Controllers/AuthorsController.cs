using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly BlogContext _context;

        public AuthorsController(BlogContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.Authors);
        }
        public ViewResult Create()
        {
            return View(new Author());
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { authorId = author.Id });
        }
        public ViewResult Edit(int authorId)
        {
            return View(_context.Authors.First(a => a.Id == authorId));
        }

        [HttpPost]
        public ViewResult Edit(Author author)
        {
            _context.Authors.Attach(author);
            _context.SetModified(author);
            _context.SaveChanges();
            return View(_context.Authors.First(a => a.Id == author.Id));
        }
        public ActionResult Delete(int authorId)
        {
            var authorToDelete = _context.Authors.First(a => a.Id == authorId);
            _context.Authors.Remove(authorToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}