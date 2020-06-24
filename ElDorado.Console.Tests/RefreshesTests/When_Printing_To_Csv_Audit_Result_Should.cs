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
    public class When_Printing_To_Csv_Audit_Result_Should
    {
        private AuditResult Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new AuditResult();   
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Quotations_To_The_Title_Fields_In_Case_Of_Commas()
        {
            const string title = "A Man, A Plan, A Canal, Panama";
            const string url = "https://www.somesitem.com";

            var result = new PageCheckResult() { PageUrl = url, PageTitle = title };
            result.AddIssue("Oops!");

            Target.AddPageCheckResult(result);

            Target.ProblemsToCsv().ShouldContain($"\"{title}\"");
        }
}
}
