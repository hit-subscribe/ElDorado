using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Checking_For_Problematic_Terms_PageChecker_Should
    {
        private static readonly string FirstUrl = "https://somesite.com/whatever";

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
            Target = new PageChecker(Client) { ProblemTerms = new List<string>() { "master", "slave" } };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Slave_When_Page_Contains_Text_Slave()
        {
            ArrangePage("<html><body>slave</body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain("Contains term \"slave\"");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Page_Contains_Harmless_Text()
        {
            ArrangePage("<html><body>salve</body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Problem_When_Page_Contains_Text_Master()
        {
            ArrangePage("<html><body>master</body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Contains term \"master\"");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Problem_When_Problem_Word_In_Html_Has_Different_Casing()
        {
            ArrangePage("<html><body>mAsTeR</body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Contains term \"master\"");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_No_Problems_When_Word_Is_Part_Of_Larger_Word()
        {
            ArrangePage("<html><body>masterful</body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Problem_When_Word_Succeeded_By_Punctuation()
        {
            ArrangePage("<html><body>master.</body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Contains term \"master\"");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Ignore_Anything_Not_Inside_The_Body()
        {
            ArrangePage("<html>masterful<body></body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Ignore_A_Term_Inside_A_Url()
        {
            ArrangePage("<html><body><a href=\"https://blah.com/master/slave/whatever.html\">asdf</a></body></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Nothing_When_No_Body_Exists()
        {
            ArrangePage("<html></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }
}
}
