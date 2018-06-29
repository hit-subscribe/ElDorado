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
using System.Web.Mvc;
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.BlogPostsControllerTests
{
    [TestClass]
    public class When_Retrieving_The_BlogPostsIndex_BlogPostsController_Should
    {
        private const string Title = "A Thought Provoking Piece";
        private const int BlogId = 1;
        private const int AuthorId = 14;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private BlogPost Post => Context.BlogPosts.First();

        private Author Author => Context.Authors.First();

        private Blog Blog => Context.Blogs.First();

        private BlogPostsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            BlogPost blogPost = new BlogPost()
            {
                Title = Title,
                BlogId = BlogId,
                AuthorId = AuthorId,
                TargetPublicationDate = new DateTime(2018, 8, 1)
            };

            Context.BlogPosts.Add(blogPost);
            Context.Authors.Add(new Author());
            Context.Blogs.Add(new Blog());

            Target = new BlogPostsController(Context) { Today = new DateTime(2018, 7, 1) };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_Containing_Authors_For_Selection()
        {
            var viewModel = Target.Index().GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.Authors.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Include_Author_First_And_Last_Name()
        {
            Author.FirstName = "Erik";
            Author.LastName = "Dietrich";

            var viewModel = Target.Index().GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.Authors.First().Text.ShouldBe("Erik Dietrich");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Authors_Sorted_By_LastName()
        {
            Author.LastName = "Zurly";
            Context.Authors.Add(new Author() { LastName = "Asquith" });

            var viewModel = Target.Index().GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.Authors.First().Text.ShouldBe(" Asquith");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Companies_Sorted()
        {
            Blog.CompanyName = "Zephyr";
            Context.Blogs.Add(new Blog() { CompanyName = "Acme" });

            var viewModel = Target.Index().GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.Blogs.First().Text.ShouldBe("Acme");
        }

       [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_Containing_The_BlogPosts_From_Context()
        {
            var viewModel = Target.Index().GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.First().Title.ShouldBe(Title);
        }
    
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_Out_Posts_From_Non_Matching_Blogs_When_Filter_Index_Is_Invoked()
        {
            var viewModel = Target.Index(123).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Posts_From_Matching_Blogs()
        {
            var viewModel = Target.Index(BlogId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ToList().ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Posts_From_Matching_Authors()
        {
            var viewModel = Target.Index(authorId: AuthorId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_Out_Posts_Not_Matching_The_Author()
        {
            var viewModel = Target.Index(authorId: AuthorId + 1).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldBeEmpty();

        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_By_Author_And_Blog_When_Both_Are_Specified()
        {
            var viewModel = Target.Index(BlogId, AuthorId + 1).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Match_An_Author_Filter_When_There_Is_No_Author()
        {
           Post.AuthorId = null;

            var viewModel = Target.Index(BlogId, AuthorId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Match_An_Authorless_Post_When_No_Search_Criteria_Is_Specified()
        {
            Post.AuthorId = null;

            var viewModel = Target.Index(BlogId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_Out_Archived_Posts()
        {

            Post.TargetPublicationDate = Target.Today.AddDays(-1);

            var viewModel = Target.Index(BlogId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_Out_Archived_Posts_Based_On_FinalizedDate_When_No_Pub_Date_Exists()
        {
            Post.TargetPublicationDate = null;
            Post.TargetFinalizeDate = Target.Today.AddDays(+1);

            var viewModel = Target.Index(BlogId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_Out_Archived_Posts_Based_On_DraftDate_When_No_Pub_Or_Final_Date_Exists()
        {
            Post.TargetPublicationDate = null;
            Post.DraftDate = Target.Today.AddDays(-1);

            var viewModel = Target.Index(BlogId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Show_Posts_With_No_Dates_Set()
        {
            Post.TargetPublicationDate = null;

            var viewModel = Target.Index(BlogId).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_OutDated_Posts_If_Include_All_Is_True()
        {
            Post.TargetPublicationDate = null;
            Post.DraftDate = Target.Today.AddDays(-1);

            var viewModel = Target.Index(includeAll: true).GetViewResultModel<BlogPostIndexViewModel>();

            viewModel.BlogPosts.ShouldNotBeEmpty();
        }
    }
}
