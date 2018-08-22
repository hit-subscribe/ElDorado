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
    }
}