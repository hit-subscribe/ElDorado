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

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private BlogPostsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            BlogPost blogPost = new BlogPost()
            {
                Title = Title,
                BlogId = 1 
            };

            Context.BlogPosts.Add(blogPost);

            Target = new BlogPostsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_View_Containing_The_BlogPosts_From_Context()
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
}
}
