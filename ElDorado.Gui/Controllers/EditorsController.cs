using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.Controllers
{
    public class EditorsController : CrudController<Editor>
    {
        public EditorsController(BlogContext context) : base(context)
        {
            IndexSortFunction = editors => editors.OrderBy(e => e.FirstName);
        }
    }
}