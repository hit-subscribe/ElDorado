using ElDorado.WritingCalendar;
using Google.Apis.Sheets.v4;
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
    public class When_Retrieving_Planned_Posts_PlanningSpreadsheetService_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Retrieve_The_Values_From_The_Correct_Spreadsheet()
        {
            var sheetsService = Mock.Create<SheetsService>();
            sheetsService.Arrange(ss => ss.Spreadsheets.Values.Get(Arg.AnyString, Arg.AnyString)).Returns(Mock.Create<GetRequest>());
            var service = new PlanningSpreadsheetService(sheetsService);

            service.GetPlannedPosts();
        }
}
}
