using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class BlogsController : CrudController<Blog>
    {
        public BlogsController(BlogContext context) : base(context)
        {
            IndexSortFunction = blogs => blogs.OrderBy(b => b.CompanyName);
        }
    }
}