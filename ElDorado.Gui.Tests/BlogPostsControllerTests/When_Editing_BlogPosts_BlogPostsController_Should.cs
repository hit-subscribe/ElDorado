using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using ElDorado.Trello;
using ElDorado.Wordpress;
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
    public class When_Editing_BlogPosts_BlogPostsController_Should
    {
        private const string PostTitle = "A Bit of Clickbait";
        private const int PostId = 1;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private Author Author => Context.Authors.First();
        private Editor Editor => Context.Editors.First();
        private Blog Blog => Context.Blogs.First();
        
        private TrelloWritingCalendarService Service { get; set; } = Mock.Create<TrelloWritingCalendarService>();
        private WordpressService WordpressService { get; set; } = Mock.Create<WordpressService>();

        private BlogPost Post { get; set; } = new BlogPost()
        {
            Id = PostId,
            Title = PostTitle,
            AuthorId = 1,
            EditorId = 1,
            WordpressId = 1,
            BlogId = 1
        };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Blogs.Add(new Blog() { Id = 1, IsActive = true });
            Context.Authors.Add(new Author() { Id = 1 });
            Context.Editors.Add(new Editor() { Id = 1 });
            Context.BlogPosts.Add(Post);

            Context.Arrange(ctx => ctx.UpdateBlogPostDependencies(Arg.IsAny<BlogPost>())).DoInstead((BlogPost bp) => bp.Author = Context.Authors.First(a => a.Id == bp.AuthorId)).DoInstead((BlogPost bp) => bp.Editor = Context.Editors.First(e => e.Id == bp.EditorId));

            Target = new BlogPostsController(Context, Service, WordpressService) { MapPath = "asdf" };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_View_Containing_A_Single_BlogPost()
        {
            var viewModel = Target.Edit(PostId).GetResult<BlogPostEditViewModel>();

            viewModel.Post.Title.ShouldBe(PostTitle);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_All_Blogs()
        {
            Blog blog = new Blog() { CompanyName = "Acme", IsActive = true };
            Context.Blogs.Add(blog);

            var viewModel = Target.Edit(PostId).GetResult<BlogPostEditViewModel>();

            viewModel.Blogs.ShouldContain(item => item.Text == blog.CompanyName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_InactiveAuthors_Out_Of_AuthorsList()
        {
            Author.IsActive = false;

            var viewModel = Target.Edit(PostId).GetResult<BlogPostEditViewModel>();

            viewModel.Authors.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_All_Authors()
        {
            var author = new Author() { Id = 12 };
            Context.Authors.Add(author);

            var viewModel = Target.Edit(PostId).GetResult<BlogPostEditViewModel>();

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
        public void Redirect_To_Edit_On_Postback()
        {
            var actionResult = Target.Edit(new BlogPostEditViewModel(Post, Context)) as RedirectToRouteResult;

            actionResult.ShouldHaveRouteAction("Edit");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_The_Card_In_The_TrelloService()
        {
            Service.Arrange(s => s.EditCard(Post));

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            Service.Assert(s => s.EditCard(Post), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Author_Pay()
        {
            Author.BaseRate = 150;
            Target.Edit(new BlogPostEditViewModel(new BlogPost() { AuthorId = Author.Id, EditorId = Editor.Id, IsDoublePost = true, IsGhostwritten = true }, Context) { AuthorPay = null });

            Context.BlogPosts.Last().AuthorPay.ShouldBe(450);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Editor_Pay()
        {
            Editor.BaseRate = 0.04M;

            Target.Edit(new BlogPostEditViewModel(new BlogPost() { AuthorId = Author.Id, EditorId = Editor.Id, Content = "<p>asdf</p>" }, Context) { EditorPay = null });

            Context.BlogPosts.Last().EditorPay.ShouldBe(0.04M);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Use_User_Specified_Editor_Pay_When_Specified()
        {
            const decimal userSpecifiedPay = 1.23M;

            Editor.BaseRate = 0.04M;

            Target.Edit(new BlogPostEditViewModel(new BlogPost() { AuthorId = Author.Id, EditorId = Editor.Id, Content = "<p>asdf</p>" }, Context) { EditorPay = userSpecifiedPay });

            Context.BlogPosts.Last().EditorPay.ShouldBe(userSpecifiedPay);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Sync_To_Wordpress()
        {
            WordpressService.Arrange(ws => ws.SyncToWordpress(Post));

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            WordpressService.Assert(ws => ws.SyncToWordpress(Post), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Authorize_Wordpress()
        {
            WordpressService.Arrange(ws => ws.AuthorizeUser(Arg.AnyString));

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            WordpressService.Assert(ws => ws.AuthorizeUser(Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Model_Error_For_Wordpress_Author_When_Sync_Throws_MissingAuthorException()
        {
            WordpressService.Arrange(wps => wps.SyncToWordpress(Arg.IsAny<BlogPost>())).Throws<MissingAuthorException>();

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            Target.ModelState["Post.AuthorId"].Errors.First().ErrorMessage.ShouldBe("Author not in Wordpress.");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Same_View_When_Model_State_Is_Invalid()
        {
            WordpressService.Arrange(wps => wps.SyncToWordpress(Arg.IsAny<BlogPost>())).Throws<MissingAuthorException>();

            var result = Target.Edit(new BlogPostEditViewModel(Post, Context)) as ViewResult;

            result.ShouldNotBeNull();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Only_Sync_To_Wordpress_When_Post_Has_WordpressId()
        {
            WordpressService.Arrange(ws => ws.SyncToWordpress(Post));

            Post.WordpressId = 0;

            Target.Edit(new BlogPostEditViewModel(Post, Context));

            WordpressService.Assert(ws => ws.SyncToWordpress(Post), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Include_The_Post_Blog_Even_If_That_Client_Is_Inactive()
        {
            Blog.IsActive = false;
            
            var viewModel = Target.Edit(PostId).GetResult<BlogPostEditViewModel>();

            viewModel.Blogs.ShouldContain(item => item.Value == Blog.Id.ToString());
        }
}
}
