using ElDorado.Domain;
using ElDorado.Gui.Controllers;
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
    public class When_Providing_HustlingEmail_ReportsController_Should
    {
        public BlogPost Post { get; set; } = new BlogPost()
        {
            IsApproved = true,
            DraftDate = Today.AddDays(1),
            Title = "A Blog Post",
        };

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();
        private ReportsController Target { get; set; }

        private static readonly DateTime Today = new DateTime(2019, 10, 14);

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.BlogPosts.Add(Post);
            Target = new ReportsController(Context) { Today = Today };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_An_Empty_Collection_Of_Posts_When_There_Are_No_Authorless_Posts()
        {
            Post.Author = new Author() { FirstName = "Erik", LastName = "Dietrich" };

            var records = Target.HustlingEmail().GetResult<IEnumerable<BlogPost>>();

            records.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Blog_Post_When_That_Post_Has_No_Author_And_Is_In_The_Future()
        {
            var records = Target.HustlingEmail().GetResult<IEnumerable<BlogPost>>();

            records.First().Title.ShouldBe(Post.Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_When_Authorless_But_Complete_In_The_Past()
        {
            Post.DraftCompleteDate = Today.AddDays(-1);

            var records = Target.HustlingEmail().GetResult<IEnumerable<BlogPost>>();

            records.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_When_DraftDueDate_Is_In_The_Past()
        {
            Post.DraftDate = Today.AddDays(-1);

            var records = Target.HustlingEmail().GetResult<IEnumerable<BlogPost>>();

            records.ShouldBeEmpty();

        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Returns_Post_When_DraftDue_Is_Null()
        {
            Post.DraftDate = null;

            var records = Target.HustlingEmail().GetResult<IEnumerable<BlogPost>>();

            records.First().Title.ShouldBe(Post.Title);

        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Returns_Nothing_If_Post_Is_Not_Approved()
        {
            Post.IsApproved = false;

            var records = Target.HustlingEmail().GetResult<IEnumerable<BlogPost>>();

            records.ShouldBeEmpty();
        }
}
}
