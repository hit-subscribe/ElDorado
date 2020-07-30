using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Getting_A_PageResult_Async_AuditService_Should
    {
        private SimpleWebClient Client { get; } = Mock.Create<SimpleWebClient>();

        private AuditService Target { get; set; }

        private void ArrangePage(string html)
        {
            Client.Arrange(c => c.GetRawResultOfBasicGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(html));
        }

        private PageCheckResult TheResult  => Target.GetPageResultAsync(string.Empty).Result;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new AuditService(Client);

            Target.Now = new DateTime(2020, 1, 1);
        }


        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_Issues_List_For_Empty_Page()
        {
            ArrangePage(string.Empty);

            TheResult.Issues.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_An_Issue_When_Title_Contains_A_Previous_Year()
        {
            ArrangePage("<html><title>Best Stuff in 2019</title></html>");

            TheResult.Issues.ShouldContain(i => i == "Is possibly outdated");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_The_Title_To_The_PageResult()
        {
            const string title = "Best Stuff in 2019";
            ArrangePage($"<html><title>{title}</title></html>");

            TheResult.PageTitle.ShouldBe(title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Flag_Title_From_Several_Years_Ago()
        {
            ArrangePage("<html><title>Best Stuff in 2016</title></html>");

            TheResult.Issues.ShouldContain(i => i == "Is possibly outdated");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_Year_Before_2000()
        {
            ArrangePage("<html><title>Best Stuff in 1999</title></html>");

            TheResult.Issues.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_A_Title_From_Current_Year()
        {
            ArrangePage($"<html><title>Best Stuff in {Target.Now.Year}</title></html>");

            TheResult.Issues.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_Year_In_The_Future()
        {
            ArrangePage($"<html><title>Best Stuff in {Target.Now.Year + 10}</title></html>");

            TheResult.Issues.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Flag_An_H1_From_Last_Year()
        {
            ArrangePage($"<html><h1>Best Stuff in {Target.Now.Year - 1}</h1></html>");

            TheResult.Issues.ShouldContain(i => i == "Is possibly outdated");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Flag_H1_With_Current_Year()
        {
            ArrangePage($"<html><h1>Best Stuff in {Target.Now.Year}</h1></html>");

            TheResult.Issues.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Include_Title_Regardless_Of_NestingDepth()
        {
            const string title = "Some Post Title";
            ArrangePage($"<html><blah><title>{title}</title></blah><body><h1>Best Stuff in 2019</h1></body></html>");

            TheResult.PageTitle.ShouldBe(title);
        }
    }

}
