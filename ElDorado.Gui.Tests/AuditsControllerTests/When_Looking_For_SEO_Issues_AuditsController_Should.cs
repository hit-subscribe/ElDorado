using ElDorado.Gui.Controllers;
using ElDorado.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.AuditsControllerTests
{
    [TestClass]
    public class When_Looking_For_SEO_Issues_AuditsController_Should
    {
        private AuditResult Result { get; set; } = new AuditResult();

        private PageChecker Checker { get; set; } = Mock.Create<PageChecker>();

        private AuditsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Checker.Arrange(c => c.AuditFromSitemapUrl(Arg.AnyString)).Returns(Result);

            Target = new AuditsController(Checker);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_PageCheckResults_From_A_Post_To_SeoAuditResults()
        {
            var pageCheckResults = Target.SeoCheck("https://site.com/sitemap.xml").GetResult<IEnumerable<PageCheckResult>>();

            pageCheckResults.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Use_SeoAuditResults_On_Postback()
        {
            var viewResult = Target.SeoCheck("https://site.com/sitemap.xml");

            viewResult.ViewName.ShouldBe("SeoAuditResults");
        }
}

}
