using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.JustMock.EntityFramework;

namespace ElDorado.Gui.Tests.ReportsControllerTests
{
    [TestClass]
    public class When_Providing_PostHustlingReport_ReportsController_Should
    {
        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private ReportsController Target { get; set; }

        private PostHustlingViewModel GetPostHustlingViewModel() => Target.PostHustling().GetViewResultModel<PostHustlingViewModel>();

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new ReportsController(Context) { Today = new DateTime(2018, 9, 1) };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Authorless_Posts()
        {
            Context.BlogPosts.Add(new BlogPost() { DraftDate = Target.Today.AddDays(1), IsApproved = true });

            var viewModel = GetPostHustlingViewModel();

            viewModel.UnclaimedPosts.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_That_Filters_Out_A_Post_With_An_Author()
        {
            var post = new BlogPost() { Author = new Author() { Id = 1 } };
            Context.BlogPosts.Add(post);

            var viewModel = GetPostHustlingViewModel();

            viewModel.UnclaimedPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Past_Posts_Filtered_Out()
        {
            Context.BlogPosts.Add(new BlogPost() { DraftDate = Target.Today.AddDays(-1) } );

            var viewModel = GetPostHustlingViewModel();

            viewModel.UnclaimedPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Unapproved_Posts_Filtered_Out()
        {
            Context.BlogPosts.Add(new BlogPost() { DraftDate = Target.Today.AddDays(1) });

            var viewModel = GetPostHustlingViewModel();

            viewModel.UnclaimedPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Posts_Sorted_By_Due_Date()
        {
            var firstChronologicalPost = new BlogPost() { DraftDate = Target.Today.AddDays(1), IsApproved = true };
            var secondChronologicalPost = new BlogPost() { DraftDate = Target.Today.AddDays(2), IsApproved = true };

            Context.BlogPosts.Add(secondChronologicalPost);
            Context.BlogPosts.Add(firstChronologicalPost);

            var viewModel = GetPostHustlingViewModel();

            viewModel.UnclaimedPosts.First().DraftDate.ShouldBe(firstChronologicalPost.DraftDate);
        }
}
}
