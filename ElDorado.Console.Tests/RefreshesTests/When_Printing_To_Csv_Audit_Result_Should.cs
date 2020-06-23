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
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Quotations_To_The_Title_Fields_In_Case_Of_Commas()
        {
            var title = "A Man, A Plan, A Canal, Panama";
            var page = new Page($"<html><title>{title}</title></html>", "https://www.somesitem.com");

            var target = new AuditResult();

            target.AddProblem(page, "Oops!");

            target.ToCsv().ShouldContain($"\"{title}\"");
        }
}
}
