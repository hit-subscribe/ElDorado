using ElDorado.Console.Refreshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class AuditsController : Controller
    {
        private PageChecker _checker;

        public AuditsController(PageChecker checker)
        {
            _checker = checker;
        }

        public ViewResult SeoCheck()
        {
            return View();
        }

        [HttpPost]
        public ViewResult SeoCheck(string sitemapPath)
        {
            var auditResult = _checker.AuditFromSitemapUrl(sitemapPath);

            return View("SeoAuditResults", auditResult.PageResults);
        }
    }
}