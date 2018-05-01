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
    public class When_Adding_Posts_To_The_Spreadsheet_AddPosts_Should
    {

        private const string CompanyName = "Some Tech Company";
        private const string Title = "A Blog Post Title";
        private const string DraftDate = "12/15/2018";
        private const string FinalizedDate = "12/22/2018";
        private const string PublicationDate = "12/26/2018";
        private const string AuthorName = "Erik";
        private const string Keyword = "C# Goodies";
        private const string Mission = "To boldly go where no one has gone before.";
        private const string Id = "12";

        private readonly IList<IList<object>> ExistingSpreadsheetRows = new List<IList<object>>() { new List<object>() { CompanyName, Title, null, null, Mission, AuthorName, DraftDate, FinalizedDate, PublicationDate, Keyword, null, null, null, null, null, null, null, "Yes", "Yes", Id } };


        private GoogleSheet Sheet { get; } = Mock.Create<GoogleSheet>();

        private PlanningSpreadsheetService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Sheet.Arrange(s => s.GetCells(Arg.AnyString)).Returns(ExistingSpreadsheetRows);

            Target = new PlanningSpreadsheetService(Sheet);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Call_UpdateSpreadsheet_With_A_One_Additional_Row()
        {
            Target.AddPosts(new BlogPost());

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(rows => rows.Count == 2)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Call_Update_Spreadsheet_With_A_Row_20_Wide()
        {
            Target.AddPosts(new BlogPost());

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(rows => rows[0].Count == 20)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Call_Update_With_Title_In_Position_1()
        {
            Target.AddPosts(new BlogPost() { DraftDate = new DateTime(2020, 12, 12) });

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(rows => rows[0][1].ToString() == Title)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Call_Update_With_CompanyName_In_Position_0()
        {
            Target.AddPosts(new BlogPost() { DraftDate = new DateTime(2020, 12, 12) });

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(rows => rows[0][0].ToString() == CompanyName)));
        }

    [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Insert_The_Post_In_Date_Order()
        {
            DateTime earlierDateThanExistingPost = new DateTime(2016, 12, 12);
            var post = new BlogPost() { Title = "asdf", DraftDate = earlierDateThanExistingPost };

            Target.AddPosts(post);

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(rows => rows[0][1].ToString() == post.Title)));
        }
}
}
