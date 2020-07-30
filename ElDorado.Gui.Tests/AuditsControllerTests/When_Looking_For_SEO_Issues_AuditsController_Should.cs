using ElDorado.Gui.Controllers;
using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using ElDorado.Gui.ViewModels;

namespace ElDorado.Gui.Tests.AuditsControllerTests
{
    [TestClass]
    public class When_Looking_For_SEO_Issues_AuditsController_Should
    {
        private AuditResult Result { get; set; } = new AuditResult();

        private SitemapService SitemapService { get; set; } = Mock.Create<SitemapService>();

        private AuditService AuditService { get; set; } = Mock.Create<AuditService>();

        private AuditsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new AuditsController(SitemapService, AuditService);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_PageCheckResults_From_A_Post_To_SeoAuditResults()
        {
            var pageCheckResults = Target.SeoCheck("https://site.com/sitemap.xml").Result.GetResult<IEnumerable<PageCheckViewModel>>();

            pageCheckResults.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Use_SeoAuditResults_On_Postback()
        {
            var viewResult = Target.SeoCheck("https://site.com/sitemap.xml").Result;

            viewResult.ViewName.ShouldBe("SeoAuditResults");
        }
    }

}
