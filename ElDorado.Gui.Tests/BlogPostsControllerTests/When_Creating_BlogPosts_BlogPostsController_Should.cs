using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.BlogPostsControllerTests
{
    [TestClass]
    public class When_Creating_BlogPosts_BlogPostsController_Should
    {
        private const string PostTitle = "Clickbait FTW";
        private const int PostId = 12;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogPostsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Get_Request_With_BlogPostViewModel_Containing_Empty_BlogPost()
        {
            var viewModel = Target.Create().GetViewResultModel<BlogPostViewModel>();

            viewModel.Post.Id.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Get_Request_With_A_ViewMode_Containing_All_Blogs()
        {
            Blog blog = new Blog() { CompanyName = "EvilCorp" };
            Context.Blogs.Add(blog);

            var viewModel = Target.Create().GetViewResultModel<BlogPostViewModel>();

            viewModel.Blogs.ShouldContain(item => item.Text == blog.CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_To_Context_On_Postback()
        {
            Target.Create(Post);

            Context.BlogPosts.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Save_On_Postback()
        {
            bool wasCalled = false; 
            Context.Arrange(ctx => ctx.SaveChanges()).DoInstead(() => wasCalled = true);

            Target.Create(Post);

            wasCalled.ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_To_Edit_On_Postback()
        {
            var actionResult = Target.Create(Post) as RedirectToRouteResult;

            actionResult.ShouldHaveRouteAction("Edit");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_To_Edit_On_Postback_With_PostId_Set()
        {
            var actionResult = Target.Create(Post) as RedirectToRouteResult;

            actionResult.ShouldHaveRouteParameter("postId", PostId);
        }
    }
}
