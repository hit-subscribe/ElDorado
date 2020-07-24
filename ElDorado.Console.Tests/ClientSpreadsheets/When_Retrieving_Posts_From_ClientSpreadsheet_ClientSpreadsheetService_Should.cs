using ElDorado.Console.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.ClientSpreadsheets
{
    [TestClass]
    public class When_Retrieving_Posts_From_ClientSpreadsheet_ClientSpreadsheetService_Should
    {
        private static readonly DateTime PubDate = new DateTime(2017, 8, 4);
        private const string Title = "A Blog Post Title";
        private const string Mission = "Impossible";

        private readonly IList<IList<object>> AllSpreadsheetRows = new List<IList<object>>() { new List<object>() { Title, null, null, Mission, PubDate.ToString() } };

        private GoogleSheet Sheet { get; } = Mock.Create<GoogleSheet>();

        private ClientSpreadsheetService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Sheet.Arrange(gs => gs.GetCells(Arg.AnyString)).Returns(AllSpreadsheetRows);

            Target = new ClientSpreadsheetService(Sheet);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Blog_Post_With_Title_Set_To_Column_0()
        {
            var firstPost = Target.GetPosts().First();

            firstPost.Title.ShouldBe(Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Use_The_Range_Passed_In_To_GetPosts()
        {
            const string range = "Whatever";

            var firstPost = Target.GetPosts(range).First();

            Sheet.Assert(gs => gs.GetCells(range), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Mission_Set_To_Column_3()
        {
            var firstPost = Target.GetPosts().First();

            firstPost.Mission.ShouldBe(Mission);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Estimated_PubDate_Set_To_Column_4()
        {
            var firstPost = Target.GetPosts().First();

            firstPost.TargetPublicationDate.ShouldBe(PubDate);
        }
    }
}
