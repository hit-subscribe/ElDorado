using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.Controllers
{
    public class BlogsController : CrudController<Blog>
    {
        public BlogsController(BlogContext context) : base(context) { }
    }
}