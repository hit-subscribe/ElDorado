using ElDorado.Console.Refreshes;
using ElDorado.Console.Trello;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Logging_Problems_AuditResult_Should
    {
        private AuditResult Target { get; set; }

        private static readonly string FirstUrl = "https://daedtech.com/whatever-post";
        private static readonly string SecondUrl = "https://daedtech/com/some-different-post";

        private static readonly string BadPostProblem = "This is a terrible post.";
        private static readonly string OutdatedProblem = "This post is as old as dirt.";

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new AuditResult();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_String_When_No_Problems_Added()
        {
            Target.ProblemsToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Problem_When_One_Problem_Added()
        {
            var result = new PageCheckResult();
            result.AddIssue(BadPostProblem);

            Target.AddPageCheckResult(result);

            Target.ProblemsToCsv().ShouldContain(BadPostProblem);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Url_Comma_Problem_Comma_Problem_When_Two_Problems_Are_Added_For_A_Url()
        {
            var result = new PageCheckResult();
            result.AddIssue(BadPostProblem);
            result.AddIssue(OutdatedProblem);

            Target.AddPageCheckResult(result);

            Target.ProblemsToCsv().ShouldContain($"{BadPostProblem},{OutdatedProblem}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_New_Line_For_A_Second_Problem()
        {
            var firstResult = new PageCheckResult() { PageUrl = FirstUrl, PageTitle = "Some Page" };
            firstResult.AddIssue(BadPostProblem);

            var secondResult = new PageCheckResult() { PageUrl = SecondUrl, PageTitle = "Another Page" };
            secondResult.AddIssue(OutdatedProblem);

            Target.AddPageCheckResult(firstResult);
            Target.AddPageCheckResult(secondResult);

            Target.ProblemsToCsv().ShouldContain($"{Environment.NewLine}{SecondUrl}");
        }

    }

}
