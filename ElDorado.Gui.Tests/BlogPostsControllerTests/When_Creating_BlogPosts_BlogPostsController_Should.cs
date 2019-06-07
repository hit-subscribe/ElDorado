using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using ElDorado.Trello;
using ElDorado.Wordpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.JustMock;
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
        private TrelloWritingCalendarService TrelloService = Mock.Create<TrelloWritingCalendarService>();
        private WordpressService WordpressService = Mock.Create<WordpressService>();

        private Author Author => Context.Authors.First();

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle, AuthorId = 1 };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Authors.Add(new Author() { Id = 1 });
            Context.Arrange(ctx => ctx.UpdateBlogPostDependencies(Arg.IsAny<BlogPost>())).DoInstead((BlogPost bp) => bp.Author = Context.Authors.First(a => a.Id == bp.AuthorId));

            Target = new BlogPostsController(Context, TrelloService, WordpressService) { MapPath = "somepath" };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Get_Request_With_BlogPostViewModel_Containing_Empty_BlogPost()
        {
            var viewModel = Target.Create().GetResult<BlogPostEditViewModel>();

            viewModel.Post.Id.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Get_Request_With_A_ViewModel_Containing_All_Blogs()
        {
            Blog blog = new Blog() { CompanyName = "EvilCorp" };
            Context.Blogs.Add(blog);

            var viewModel = Target.Create().GetResult<BlogPostEditViewModel>();

            viewModel.Blogs.ShouldContain(item => item.Text == blog.CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_InactiveAuthors_Out_Of_AuthorsList()
        {
            Author.IsActive = false;

            var viewModel = Target.Create().GetResult<BlogPostEditViewModel>();

            viewModel.Authors.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Get_Request_With_BlogId_Specified_By_Setting_Post_BlogId()
        {
            const int blogId = 6;
            var viewModel = Target.Create(blogId).GetResult<BlogPostEditViewModel>();

            viewModel.Post.BlogId.ShouldBe(blogId);

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
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Create(Post);

            Context.Assert(ctx => ctx.SaveChanges(), Occurs.Exactly(2));
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

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Invoke_The_TrelloService_Initialize()
        {
            TrelloService.Arrange(ts => ts.Initialize(Arg.AnyString));

            Target.Create(Post);

            TrelloService.Assert(ts => ts.Initialize(Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_To_CreateGet_When_CreateNew_Is_Set_To_CreateandAddAnother()
        {
            var actionResult = Target.Create(Post, "Create and Add Another") as RedirectToRouteResult;

            actionResult.ShouldHaveRouteAction("Create");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_To_CreateGet_With_BlogId_Parameter_When_CreateNew_Is_Set_To_CreateandAddAnother()
        {
            const int blogId = 4;
            Post.BlogId = blogId;

            var actionResult = Target.Create(Post, "Create and Add Another") as RedirectToRouteResult;

            actionResult.ShouldHaveRouteParameter("blogId", blogId);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_AuthorPay_To_Base_Author_Rate_For_Normal_Post()
        {
            Author.BaseRate = 100;
            Target.Create(new BlogPost() { AuthorId = Author.Id });

            Context.BlogPosts.First().AuthorPay.ShouldBe(100);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Author_Pay_Double_For_GhostPost()
        {
            Author.BaseRate = 100;
            Target.Create(new BlogPost() { AuthorId = Author.Id, IsGhostwritten = true });

            Context.BlogPosts.First().AuthorPay.ShouldBe(200);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Author_Pay_Double_For_2x_Post()
        {
            Author.BaseRate = 100;
            Target.Create(new BlogPost() { AuthorId = Author.Id, IsDoublePost = true });

            Context.BlogPosts.First().AuthorPay.ShouldBe(200);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Author_Pay_To_Three_Hundred_For_Ghostwritten_Double_Post()
        {
            Author.BaseRate = 150;
            Target.Create(new BlogPost() { AuthorId = Author.Id, IsDoublePost = true, IsGhostwritten = true });

            Context.BlogPosts.First().AuthorPay.ShouldBe(450);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Sync_To_Wordpress()
        {
            WordpressService.Arrange(ws => ws.SyncToWordpress(Post));

            Target.Create(Post);

            WordpressService.Assert(ws => ws.SyncToWordpress(Post), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Authorize_Wordpress()
        {
            WordpressService.Arrange(ws => ws.AuthorizeUser(Arg.AnyString));

            Target.Create(Post);

            WordpressService.Assert(ws => ws.AuthorizeUser(Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Same_View_When_Model_State_Is_Invalid()
        {
            WordpressService.Arrange(wps => wps.SyncToWordpress(Arg.IsAny<BlogPost>())).Throws<MissingAuthorException>();

            var result = Target.Create(Post) as ViewResult;

            result.ShouldNotBeNull();
        }
    }
}
