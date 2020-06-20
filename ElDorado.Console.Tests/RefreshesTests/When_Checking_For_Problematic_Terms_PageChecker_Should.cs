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
    public class When_Checking_For_Problematic_Terms_PageChecker_Should
    {
        private PageChecker Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new PageChecker() { ProblemTerms = new List<string>() { "master", "slave" } };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Slave_When_Page_Contains_Text_Slave()
        {
            var rawPageHtml = "<html><body>slave</body></html>";

            Target.GetProblematicWords(rawPageHtml).First().ShouldBe("slave");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Page_Contains_Harmless_Text()
        {
            var rawPageHtml = "<html><body>salve</body></html>";

            Target.GetProblematicWords(rawPageHtml).ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Page_Contains_Text_Master()
        {
            var rawPageHtml = "<html><body>master</body></html>";

            Target.GetProblematicWords(rawPageHtml).First().ShouldBe("master");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_A_Problem_Word_In_Html_Has_Different_Casing()
        {
            var rawPageHtml = "<html><body>mAsTeR</body></html>";

            Target.GetProblematicWords(rawPageHtml).First().ShouldBe("master");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Nothing_When_Word_Is_Part_Of_Larger_Word()
        {
            var rawPageHtml = "<html><body>masterful</body></html>";

            Target.GetProblematicWords(rawPageHtml).ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Master_When_Succeeded_By_Punctuation()
        {
            var rawPageHtml = "<html><body>master.</body></html>";

            Target.GetProblematicWords(rawPageHtml).First().ShouldBe("master");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Ignore_Anything_Not_Inside_The_Body()
        {
            var rawPageHtml = "<html>masterful<body></body></html>";

            Target.GetProblematicWords(rawPageHtml).ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Ignore_A_Term_Inside_A_Url()
        {
            var rawPageHtml = "<html><body><a href=\"https://blah.com/master/slave/whatever.html\">asdf</a></body></html>";

            Target.GetProblematicWords(rawPageHtml).ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Nothing_When_No_Body_Exists()
        {
            var rawPageHtml = "<html></html>";

            Target.GetProblematicWords(rawPageHtml).ShouldBeEmpty();
        }
}
}
