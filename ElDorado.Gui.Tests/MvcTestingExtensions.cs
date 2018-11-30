using Shouldly;
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
        public static T GetResult<T>(this ActionResult target) where T : class
        {
            var viewResult = target as ViewResult;
            return viewResult.Model as T;
        }

        public static void ShouldHaveRouteAction(this RedirectToRouteResult target, string expected)
        {
            target.RouteValues["action"].ShouldBe(expected);
        }

        public static void ShouldHaveRouteParameter<T>(this RedirectToRouteResult target, string parameterName, T parameterValue)
        {
            target.RouteValues[parameterName].ShouldBe(parameterValue);
        }
    }
}
