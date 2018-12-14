using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class AuthorsController : CrudController<Author>
    {
        public AuthorsController(BlogContext context) : base(context)
        {
            IndexSortFunction = authors => authors.OrderBy(a => a.FirstName);
        }
    }
}