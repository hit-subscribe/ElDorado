using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using ElDorado.Trello;
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

        private Author Author => Context.Authors.First();

        private TrelloWritingCalendarService Service { get; set; } = Mock.Create<TrelloWritingCalendarService>();

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Authors.Add(new Author());
            Context.BlogPosts.Add(Post);

            Target = new BlogPostsController(Context, Service) { MapPath = "asdf" };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_View_Containing_A_Single_BlogPost()
        {
            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostEditViewModel>();

            viewModel.Post.Title.ShouldBe(PostTitle);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_All_Blogs()
        {
            Blog blog = new Blog() { CompanyName = "Acme" };
            Context.Blogs.Add(blog);

            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostEditViewModel>();

            viewModel.Blogs.ShouldContain(item => item.Text == blog.CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_InactiveAuthors_Out_Of_AuthorsList()
        {
            Author.IsActive = false;

            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostEditViewModel>();

            viewModel.Authors.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_All_Authors()
        {
            var author = new Author() { Id = 12 };
            Context.Authors.Add(author);

            var viewModel = Target.Edit(PostId).GetViewResultModel<BlogPostEditViewModel>();

            viewModel.Authors.ShouldContain(item => item.Value == author.Id.ToString());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Save_To_Context_On_Post()
        {
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            Context.Assert(ctx => ctx.SaveChanges());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Same_Id_On_Post()
        {
            var viewModel = Target.Edit(new BlogPostEditViewModel(Post, Context)).GetViewResultModel<BlogPostEditViewModel>();

            viewModel.Post.Id.ShouldBe(PostId);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_The_Card_In_The_TrelloService()
        {
            Service.Arrange(s => s.EditCard(Post));

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            Service.Assert(s => s.EditCard(Post), Occurs.Once());
        }
}
}
