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
    public class When_Providing_AccountsPayableReport_ReportsController_Should
    {

        private const decimal BaseAuthorRate = 100;
        private const decimal BaseEditorRate = 0.04M;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private ReportsController Target { get; set; }

        private static readonly BlogPost Post = new BlogPost()
        {
            DraftCompleteDate = new DateTime(2018, 12, 10),
            Content = "<p>A four word post.</p>"
        };

        private Author AuthorWithOnePost = new Author()
        {
            FirstName = "Erik",
            LastName = "Dietrich",
            BlogPosts = new List<BlogPost>() { Post },
            BaseRate = BaseAuthorRate,
        };

        private Editor EditorWithOnePost = new Editor()
        {
            FirstName = "Amanda",
            LastName = "Muledy",
            BlogPosts = new List<BlogPost>() { Post },
            BaseRate = BaseEditorRate
        };

        private AccountsPayableViewModel GetViewModel(DateTime? userPickedDate = null) => Target.AccountsPayable(userPickedDate).GetResult<AccountsPayableViewModel>();

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Authors.Add(AuthorWithOnePost);
            Context.Editors.Add(EditorWithOnePost);
            Post.Author = AuthorWithOnePost;
            Post.Editor = EditorWithOnePost;
            Post.CalculateAuthorPay();
            Post.CalculateEditorPay();

            Target = new ReportsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_AuthorLedgers_When_There_Are_No_Authors()
        {
            Context.Authors.Remove(AuthorWithOnePost);

            var viewModel = GetViewModel();

            viewModel.AuthorLedgers.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_Editor_Ledgers_When_There_Are_No_Editors()
        {
            Context.Editors.Remove(EditorWithOnePost);

            var viewModel = GetViewModel();

            viewModel.EditorLedgers.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_One_Author_Ledger_When_Author_Had_A_Post_For_The_Month()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorLedgers.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_One_Editor_When_Editor_Had_A_Post()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorLedgers.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_Author_Ledgers_When_DraftCompleteDate_Is_Last_Month()
        {
            var viewModel = GetViewModel(new DateTime(2018, 11, 1));

            viewModel.AuthorLedgers.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_No_Editor_Ledgers_When_DraftCompleteDate_Is_Last_Month()
        {
            var viewModel = GetViewModel(new DateTime(2018, 11, 1));

            viewModel.EditorLedgers.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Matching_Author_Name()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorLedgers.First().Name.ShouldBe($"{AuthorWithOnePost.FirstName} {AuthorWithOnePost.LastName}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Matching_Editor_Name()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorLedgers.First().Name.ShouldBe($"{EditorWithOnePost.FirstName} {EditorWithOnePost.LastName}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Listing_The_Blog_Post_Name()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorLedgers.First().Posts.First().Title.ShouldBe(Post.Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Listing_PostName_For_Editor()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorLedgers.First().Posts.First().Title.ShouldBe(Post.Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Listing_The_Post_Cost()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorLedgers.First().Posts.First().Cost.ShouldBe(BaseAuthorRate);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_An_Editor_Ledger_Listing_Post_Cost()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorLedgers.First().Posts.First().Cost.ShouldBe(BaseEditorRate * Post.Content.WordCount());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Ledger_Summarizing_Post_Costs_For_The_Author()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorLedgers.First().Total.ShouldBe(BaseAuthorRate);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_An_Editor_Ledger_Summarizing_Post_Costs_For_Editor()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorLedgers.First().Total.ShouldBe(BaseEditorRate * Post.Content.WordCount());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_Totaling_All_Author_Ledgers()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorTotal.ShouldBe(BaseAuthorRate);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_Totaling_All_Editor_Ledgers()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorTotal.ShouldBe(BaseEditorRate * Post.Content.WordCount());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Order_AuthorLedgers_By_AuthorFirstName()
        {
            var secondAuthor = new Author()
            {
                FirstName = "AAAA",
                LastName = "whatever",
                BlogPosts = new List<BlogPost>()
                {
                    new BlogPost()
                    {
                        DraftCompleteDate = new DateTime(2018, 12, 10),
                    }
                },
                BaseRate = BaseAuthorRate,
            };

            Context.Authors.Add(secondAuthor);

            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.AuthorLedgers.First().Name.ShouldBe($"{secondAuthor.FirstName} {secondAuthor.LastName}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Order_Editor_Ledgers_By_EditorFirstName()
        {
            var secondEditor = new Editor()
            {
                FirstName = "AAAA",
                LastName = "whatever",
                BlogPosts = new List<BlogPost>()
                {
                    new BlogPost()
                    {
                        DraftCompleteDate = new DateTime(2018, 12, 10),
                    }
                },
                BaseRate = BaseEditorRate,
            };

            Context.Editors.Add(secondEditor);

            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.EditorLedgers.First().Name.ShouldBe($"{secondEditor.FirstName} {secondEditor.LastName}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_A_GrandTotal_That_Is_Sum_Of_Authors_And_Editors()
        {
            var viewModel = GetViewModel(new DateTime(2018, 12, 1));

            viewModel.GrandTotal.ShouldBe(BaseAuthorRate + BaseEditorRate * Post.Content.WordCount());
        }
}
}
