using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElDorado.Gui.Tests
{
    public static class MvcTestingExtensions
    {
        public static T GetViewResultModel<T>(this ActionResult target) where T : class
        {
            var viewResult = target as ViewResult;
            return viewResult.Model as T;
        }
    }
}
