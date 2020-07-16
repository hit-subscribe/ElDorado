using ElDorado.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.RefreshesTests
{

    [TestClass]
    public class When_Checking_For_Bad_Links_PageChecker_Should
    {
        private const string RootDomain = "developersrus.com";
        private static readonly string FirstUrl = $"https://{RootDomain}/whatever";

        private SimpleWebClient Client { get; } = Mock.Create<SimpleWebClient>();

        private Sitemap Sitemap { get; } = Mock.Create<Sitemap>();

        private PageChecker Target { get; set; }

        private void ArrangePage(string html)
        {
            Sitemap.Arrange(sm => sm.Urls).Returns(FirstUrl.AsEnumerable());
            Client.Arrange(c => c.GetRawResultOfBasicGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(html));
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new PageChecker(Client);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_A_Problem_With_Broken_Link()
        {
            ArrangePage("<html><body><a href=\"https://brokensite.com\">Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.NotFound));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_No_Problems_When_Links_Return_200()
        {
            ArrangePage("<html><body><a href=\"https://brokensite.com\">Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.OK));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Report_Url_And_Anchor_Text_In_Problem_Report()
        {
            const string brokenLink = "https://brokensite.com";
            const string anchorText = "Oh noes!";
            ArrangePage($"<html><body><a href=\"{brokenLink}\">{anchorText}</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.NotFound));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"404 for link {brokenLink} with anchor text {anchorText}.");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Report_No_Error_For_An_A_Tag_With_No_Href()
        {
            ArrangePage("<html><body><a>Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.OK));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Report_No_Error_For_A_Tag_With_Anchor_Href()
        {
            ArrangePage("<html><body><a href=\"#\">Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.OK));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Report_Error_For_Extra_Nested_A_Href()
        {
            ArrangePage("<html><body><p><a href=\"https://brokensite.com\">Oh noes!</a></p></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.NotFound));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Ignore_Internal_Links()
        {
            ArrangePage($"<html><body><a href=\"http://{RootDomain}/another-link\">Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.NotFound));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Logs_A_Problem_For_Link_Throwing_Exception()
        {
            ArrangePage("<html><body><a href=\"http://something.com\">Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Throws<Exception>();

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Link http://something.com with anchor text Oh noes! generated an error.");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Log_No_Issue_For_Url_That_Cannot_Be_Parsed()
        {
            ArrangePage($"<html><body><a href=\"http:/r\">Oh noes!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.NotFound));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Log_Anchor_Text_Has_The_Tag_It_Contains()
        {
            const string brokenLink = "https://brokensite.com";

            ArrangePage($"<html><body><p><a href=\"{brokenLink}\"><img/></a></p></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(HttpStatusCode.NotFound));

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"404 for link {brokenLink} with anchor text img tag.");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Strip_Newlines_From_Anchor_Text_On_Errors()
        {
            ArrangePage("<html><body><a href=\"http://something.com\">Oh noes!\nAn error!</a></body></html>");
            Client.Arrange(c => c.GetHttpResponseFromGetRequestAsync(Arg.AnyString)).Throws<Exception>();

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Link http://something.com with anchor text Oh noes!An error! generated an error.");
        }
}
}
