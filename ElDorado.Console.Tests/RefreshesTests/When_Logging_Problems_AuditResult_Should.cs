using ElDorado.Refreshes;
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
            Target.ToCsv().ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Url_Comma_Problem_When_One_Problem_Added()
        {
            Target.AddProblem(new Page(string.Empty, FirstUrl), BadPostProblem);

            Target.ToCsv().ShouldContain(BadPostProblem);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Url_Comma_Problem_Comma_Problem_When_Two_Problems_Are_Added_For_A_Url()
        {
            Target.AddProblem(new Page(string.Empty, FirstUrl), BadPostProblem);
            Target.AddProblem(new Page(string.Empty, FirstUrl), OutdatedProblem);

            Target.ToCsv().ShouldContain($"{BadPostProblem},{OutdatedProblem}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_New_Line_For_A_Second_Problem()
        {
            Target.AddProblem(new Page(string.Empty, FirstUrl), BadPostProblem);
            Target.AddProblem(new Page(string.Empty, SecondUrl), OutdatedProblem);

            Target.ToCsv().ShouldContain($"{Environment.NewLine}{SecondUrl}");
        }

    }

}
