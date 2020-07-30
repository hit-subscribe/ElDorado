using ElDorado.Console;
using ElDorado.Console.Refreshes;
using ElDorado.Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class AuditsController : Controller
    {
        private PageChecker _checker;
        private SitemapService _sitemapService;
        private AuditService _auditService;

        public DateTime Now { get; set; } = DateTime.Now;

        public AuditsController(SitemapService sitemapService, AuditService auditService)
        {
            _sitemapService = sitemapService;
            _auditService = auditService;
        }

        public ViewResult SeoCheck()
        {
            return View();
        }

        [HttpPost]
        public async Task<ViewResult> SeoCheck(string sitemapPath)
        {
            var urls = _sitemapService.GetBasicUrlsFromSitemap(sitemapPath);

            var pageResults = await _auditService.GetPageResultsAsync(urls);

            var viewModels = pageResults.Select(pr => new PageCheckViewModel(pr));

            return View("SeoAuditResults", viewModels);
        }
    }
}