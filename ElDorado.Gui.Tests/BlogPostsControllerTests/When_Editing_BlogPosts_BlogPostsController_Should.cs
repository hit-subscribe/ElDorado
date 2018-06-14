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
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.BlogPostsControllerTests
{
    [TestClass]
    public class When_Editing_BlogPosts_BlogPostsController_Should
    {
        private const string PostTitle = "A Bit of Clickbait";
        private const int PostId = 1;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.BlogPosts.Add(Post);

            Target = new BlogPostsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_View_Containing_A_Single_BlogPost()
        {
            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostViewModel>();

            viewModel.Post.Title.ShouldBe(PostTitle);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_All_Blogs()
        {
            Blog blog = new Blog() { CompanyName = "Acme" };
            Context.Blogs.Add(blog);

            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostViewModel>();

            viewModel.Blogs.ShouldContain(item => item.Text == blog.CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_All_Authors()
        {
            var author = new Author() { FirstName = "Erik" };
            Context.Authors.Add(author);

            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostViewModel>();

            viewModel.Authors.ShouldContain(item => item.Text == author.FirstName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Save_To_Context_On_Post()
        {
            bool wasCalled = false; //Not sure why I can't validate the mock with Context.Assert(ctx => ctx.SaveCahnges(), Occurs.Once()), but that didn't work and it's late and I'm tired.
            Context.Arrange(ctx => ctx.SaveChanges()).DoInstead(() => wasCalled = true);

            Target.Edit(new BlogPostViewModel(Post, Context));

            wasCalled.ShouldBe(true);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Same_Id_On_Post()
        {
            var viewModel = Target.Edit(new BlogPostViewModel(Post, Context)).GetViewResultModel<BlogPostViewModel>();

            viewModel.Post.Id.ShouldBe(PostId);
        }
}
}
