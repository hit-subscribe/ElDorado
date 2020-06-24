using ElDorado.Refreshes;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Checking_A_Page_For_Possible_Outdated_Content_PageChecker_Should
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
            Target = new PageChecker(Client);

            Target.Now = new DateTime(2020, 1, 1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Page_Is_Empty()
        {
            ArrangePage("<html></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Flag_A_Title_From_A_Previous_Year()
        {
            ArrangePage("<html><title>Best Stuff in 2019</title></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain("Is possibly outdated");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Should_Add_The_Title_As_First_Column()
        {
            ArrangePage("<html><title>Best Stuff in 2019</title></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBe($"{FirstUrl},\"Best Stuff in 2019\",Is possibly outdated{Environment.NewLine}");
        }

    [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Flag_A_Title_From_Several_Year_Ago()
        {
            ArrangePage("<html><title>Best Stuff in 2016</title></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Is possibly outdated");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_A_Year_Pre_2000()
        {
            ArrangePage("<html><title>Best Stuff in 1998</title></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_Title_Year_Of_This_Year()
        {
            ArrangePage("<html><title>Best Stuff in 2020</title></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_If_Title_Year_Is_In_Future()
        {
            ArrangePage("<html><title>Best Stuff in 2070</title></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Flag_If_An_H1_Has_Last_Year()
        {
            ArrangePage("<html><h1>Best Stuff in 2019</h1></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldContain($"Is possibly outdated");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_If_H1_Has_This_Year()
        {
            ArrangePage("<html><h1>Best Stuff in 2020</h1></html>");

            var auditResult = Target.AuditSiteFromSiteMap(Sitemap).Result;

            auditResult.ProblemsToCsv().ShouldBeEmpty();
        }
    }
}
