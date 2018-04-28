using ElDorado.Domain;
using ElDorado.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
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
    public class When_Fetching_A_BlogPost_From_The_PostsSpreadsheet_PlanningSpreadsheetService_Should
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

        private readonly IList<IList<object>> AllSpreadsheetRows = new List<IList<object>>() { new List<object>() { CompanyName, Title, null, null, Mission, AuthorName, DraftDate, FinalizedDate, PublicationDate, Keyword, null, null, null, null, null, null, null, "Yes", "Yes", Id } };

        private GoogleSheet Sheet { get; } = Mock.Create<GoogleSheet>();

        private PlanningSpreadsheetService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Sheet.Arrange(gs => gs.GetCells(Arg.AnyString)).Returns(AllSpreadsheetRows);

            Target = new PlanningSpreadsheetService(Sheet);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_CompanyName_Matching_Column_0()
        {
            var post = Target.GetPosts().First();

            post.Blog.CompanyName.ShouldBe(CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Title_Matching_Column_1()
        {
            var post = Target.GetPosts().First();

            post.Title.ShouldBe(Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Skip_Adding_A_Blog_Post_That_Doesnt_Have_A_Title()
        {
            AllSpreadsheetRows[0][1] = string.Empty;

            var posts = Target.GetPosts();

            posts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_DraftDate_Set_To_Column_6()
        {
            var post = Target.GetPosts().First();

            post.DraftDate.ShouldBe(DateTime.Parse(DraftDate));
        }
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_IsApproved_True_When_Column_17_Is_Yes()
        {
            var post = Target.GetPosts().First();

            post.IsApproved.ShouldBe(true);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_IsDoublePost_True_When_Column_18_Is_Yes()
        {
            var post = Target.GetPosts().First();

            post.IsDoublePost.ShouldBe(true);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Blog_Post_With_Author_Set_To_Column_()
        {
            var post = Target.GetPosts().First();

            post.Author.FirstName.ShouldBe(AuthorName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_FinalizedDate()
        {
            var post = Target.GetPosts().First();

            post.TargetFinalizeDate.ShouldBe(DateTime.Parse(FinalizedDate));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Null_FinalizedDate_When_FinalizedDate_Is_Null()
        {
            AllSpreadsheetRows[0][7] = null;

            var post = Target.GetPosts().First();

            post.TargetFinalizeDate.ShouldBe(null);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_TargetPublicationDate_Set()
        {
            var post = Target.GetPosts().First();

            post.TargetPublicationDate.ShouldBe(DateTime.Parse(PublicationDate));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Blog_Post_With_Keyword_Set()
        {
            var post = Target.GetPosts().First();

            post.Keyword.ShouldBe(Keyword);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Mission_Set()
        {
            var post = Target.GetPosts().First();

            post.Mission.ShouldBe(Mission);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Id_Set()
        {
            var post = Target.GetPosts().First();

            post.Id.ShouldBe(int.Parse(Id));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Id_0_When_Sheet_Is_Empty()
        {
            AllSpreadsheetRows[0][19] = string.Empty;

            var post = Target.GetPosts().First();

            post.Id.ShouldBe(0);
        }
    }
}
