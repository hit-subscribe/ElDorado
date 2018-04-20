using ElDorado.Domain;
using ElDorado.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.WritingCalendar
{
    [TestClass]
    public class When_UpdatingPostIds_On_The_PlanningSpreadsheet_UpdatePostIds_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_The_Spreadsheet_With_A_Row_20_Columns_Wide()
        {
            var sheet = Mock.Create<GoogleSheet>();
            sheet.Arrange(s => s.GetCells(Arg.AnyString)).Returns(new List<IList<object>>() { new List<object>() { "asdf" } });

            var target = new PlanningSpreadsheetService(sheet);

            target.UpdatePostIds(new BlogPost().AsEnumerable());

            sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(g => g[0].Count == 20)));
        }
}
}
