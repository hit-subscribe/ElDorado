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
    public class When_Providing_AccountsReceivableReport_ReportsController_Should
    {

        private const decimal PostBaseRate = 100;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private ReportsController Target { get; set; }

        private Author AuthorWithOnePost = new Author()
        {
            FirstName = "Erik",
            LastName = "Dietrich",
            BlogPosts = new List<BlogPost>()
            {
                new BlogPost()
                {
                    DraftCompleteDate = new DateTime(2018, 12, 10),
                }
            },
            BaseRate = PostBaseRate,
        };

        private AccountsReceivableViewModel GetViewModel(int year = 0, int month = 0) => Target.AccountsReceivable(year, month).GetResult<AccountsReceivableViewModel>();

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Authors.Add(AuthorWithOnePost);
            AuthorWithOnePost.BlogPosts.First().Author = AuthorWithOnePost;

            Target = new ReportsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_Records_When_There_Are_No_Authors()
        {
            Context.Authors.Remove(AuthorWithOnePost);

            var viewModel = GetViewModel();

            viewModel.AuthorLedgers.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_One_Author_Ledger_When_Author_Had_A_Post_For_The_Month()
        {
            var viewModel = GetViewModel(2018, 12);

            viewModel.AuthorLedgers.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_Author_Ledgers_When_DraftCompleteDate_Is_Last_Month()
        {
            var viewModel = GetViewModel(2018, 11);

            viewModel.AuthorLedgers.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Matching_Author_Name()
        {
            var viewModel = GetViewModel(2018, 12);

            viewModel.AuthorLedgers.First().Name.ShouldBe($"{AuthorWithOnePost.FirstName} {AuthorWithOnePost.LastName}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Listing_The_Blog_Post_Name()
        {
            var viewModel = GetViewModel(2018, 12);

            viewModel.AuthorLedgers.First().Posts.First().Title.ShouldBe(AuthorWithOnePost.BlogPosts.First().Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Listing_The_Post_Cost()
        {
            var viewModel = GetViewModel(2018, 12);

            viewModel.AuthorLedgers.First().Posts.First().Cost.ShouldBe(PostBaseRate);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Summarizing_Post_Costs_For_The_Author()
        {
            var viewModel = GetViewModel(2018, 12);

            viewModel.AuthorLedgers.First().Total.ShouldBe(PostBaseRate);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_Totaling_All_Ledgers()
        {
            var viewModel = GetViewModel(2018, 12);

            viewModel.Total.ShouldBe(PostBaseRate);
        }
}
}
