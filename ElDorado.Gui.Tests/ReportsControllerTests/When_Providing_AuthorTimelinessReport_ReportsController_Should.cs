using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock.EntityFramework;

namespace ElDorado.Gui.Tests.ReportsControllerTests
{
    [TestClass]
    public class When_Providing_AuthorTimelinessReport_ReportsController_Should
    {
        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private ReportsController Target { get; set; }

        private static readonly DateTime Today = new DateTime(2018, 11, 1);

        private Author AuthorWithOnePost = new Author()
        {
            FirstName = "Erik",
            LastName = "Dietrich",
            BlogPosts = new List<BlogPost>()
            {
                new BlogPost()
                {
                    DraftCompleteDate = Today.AddDays(-1),
                    DraftDate = Today
                }
            }
        };

        private IEnumerable<AuthorTimelinessRecord> GetRecords() => Target.AuthorTimeliness().GetResult<IEnumerable<AuthorTimelinessRecord>>();

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Authors.Add(AuthorWithOnePost);

            Target = new ReportsController(Context) { Today = Today };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_Records_For_No_Authors()
        {
            Context.Authors.Remove(AuthorWithOnePost);

            var records = GetRecords();

            records.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_One_Record_When_There_Is_One_Author()
        {
            var records = GetRecords();

            records.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Zero_For_TotalPosts_When_Author_Has_No_Posts()
        {
            AuthorWithOnePost.BlogPosts.Clear();

            var records = GetRecords();

            records.First().PostCount.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_1_For_TotalPosts_When_Author_Has_1_Submitted_Post()
        {
            var records = GetRecords();

            records.First().PostCount.ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Record_With_Author_First_And_Last_Name()
        {
            var records = GetRecords();

            records.First().Name.ShouldBe("Erik Dietrich");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Record_With_Timeliness_Of_One_Day_Early_When_Only_Post_Is_One_Day_Early()
        {
            var records = GetRecords();

            records.First().Timeliness.ShouldBe("1 Days Early");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Record_With_Timeliness_Of_Two_Days_Early_When_OnlyPost_Is_Two_Days_Early()
        {
            AuthorWithOnePost.BlogPosts.First().DraftCompleteDate = AuthorWithOnePost.BlogPosts.First().DraftDate.Value.AddDays(-2);

            var records = GetRecords();

            records.First().Timeliness.ShouldBe("2 Days Early");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Record_With_Timeliness_Of_Two_Days_Late_When_Only_Post_Is_Two_Days_Late()
        {
            AuthorWithOnePost.BlogPosts.First().DraftCompleteDate = AuthorWithOnePost.BlogPosts.First().DraftDate.Value.AddDays(2);

            var records = GetRecords();

            records.First().Timeliness.ShouldBe("2 Days Late");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_1_For_TotalPosts_When_Author_Has_1_Post()
        {
            var records = GetRecords();

            records.First().TotalPosts.ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Truncate_Decimal_When_Applicable_For_Timeliness()
        {
            AuthorWithOnePost.BlogPosts.Add(new BlogPost() { DraftCompleteDate = Today, DraftDate = Today });
            AuthorWithOnePost.BlogPosts.Add(new BlogPost() { DraftCompleteDate = Today, DraftDate = Today });

            var records = GetRecords();

            records.First().Timeliness.ShouldBe("0.33 Days Early");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Number_Of_Late_Posts()
        {
            AuthorWithOnePost.BlogPosts.First().DraftCompleteDate = AuthorWithOnePost.BlogPosts.First().DraftDate.Value.AddDays(2);

            var records = GetRecords();

            records.First().LatePosts.ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Order_Records_By_AuthorFirstName()
        {
        	var secondAuthor = new Author()
            {
                FirstName = "AAAA",
                LastName = "Dietrich",
                BlogPosts = new List<BlogPost>()
                {
                    new BlogPost()
                    {
                        DraftCompleteDate = Today.AddDays(-1),
                        DraftDate = Today
                    }
                }
            };

            Context.Authors.Add(secondAuthor);

            var records = GetRecords();

            records.First().Name.ShouldBe($"{secondAuthor.FirstName} {secondAuthor.LastName}");
        }
    }
}
