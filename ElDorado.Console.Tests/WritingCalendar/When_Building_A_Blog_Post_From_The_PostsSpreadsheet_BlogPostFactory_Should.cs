using ElDorado.Domain;
using ElDorado.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests.WritingCalendar
{
    [TestClass]
    public class When_Building_A_Blog_Post_From_The_PostsSpreadsheet_BlogPostFactory_Should
    {
        private const string CompanyName = "Some Tech Company";
        private const string Title = "A Blog Post Title";
        private const string DraftDate = "12/15/2018";
        private const string FinalizedDate = "12/22/2018";
        private const string PublicationDate = "12/26/2018";
        private const string AuthorName = "Erik";
        private const string Keyword = "C# Goodies";
        private const string Mission = "To boldly go where no one has gone before.";

        private readonly IList<object> GoogleSheetRow = new List<object>() { CompanyName, Title, null, null, Mission, AuthorName, DraftDate, FinalizedDate, PublicationDate, Keyword, null, null, null, null, null, null, null, "Yes", "Yes" };

        private BlogPostFactory Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogPostFactory();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_CompanyName_Matching_Column_0()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.Blog.CompanyName.ShouldBe(CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Title_Matching_Column_1()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.Title.ShouldBe(Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_DraftDate_Set_To_Column_6()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.DraftDate.ShouldBe(DateTime.Parse(DraftDate));
        }
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_IsApproved_True_When_Column_17_Is_Yes()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.IsApproved.ShouldBe(true);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_IsDoublePost_True_When_Column_18_Is_Yes()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.IsDoublePost.ShouldBe(true);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Blog_Post_With_Author_Set_To_Column_()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.Author.FirstName.ShouldBe(AuthorName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_FinalizedDate()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.TargetFinalizeDate.ShouldBe(DateTime.Parse(FinalizedDate));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Null_FinalizedDate_When_FinalizedDate_Is_Null()
        {
            GoogleSheetRow[7] = null;

            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.TargetFinalizeDate.ShouldBe(null);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_TargetPublicationDate_Set()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.TargetPublicationDate.ShouldBe(DateTime.Parse(PublicationDate));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Blog_Post_With_Keyword_Set()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.Keyword.ShouldBe(Keyword);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Mission_Set()
        {
            var post = Target.MakePostFromGoogleSheetRow(GoogleSheetRow);

            post.Mission.ShouldBe(Mission);
        }

    [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_Exception_When_GoogleSheetRow_Is_Too_Short()
        {
            Should.Throw<ArgumentException>(() => Target.MakePostFromGoogleSheetRow(new List<object>())).Message.ShouldBe("googleSheetRow");
        }
    }
}
