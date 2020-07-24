using ElDorado.Console.Trello;
using ElDorado.Console.Wordpress;
using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.BlogPostsControllerTests
{
    [TestClass]
    public class When_Deleting_BlogPosts_BlogPostsController_Should
    {
        private const string PostTitle = "This One Weird Trick";
        private const int PostId = 4;
        private const int WordpressId = 123;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private WritingCalendarService Service { get; set; } = Mock.Create<WritingCalendarService>();
        private WordpressService WordpressService { get; set; } = Mock.Create<WordpressService>();


        private BlogPost Post { get; set; } = new BlogPost()
        {
            Id = PostId,
            Title = PostTitle,
            WordpressId = WordpressId
        };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.BlogPosts.Add(Post);

            Target = new BlogPostsController(Context, Service, WordpressService) { MapPath = "Doesn't matter" };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Should_Save_The_Context_To_Capture_Delete()
        {
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Delete(PostId);

            Context.Assert(ctx => ctx.SaveChanges(), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_Removal_Of_Blog_Post_From_Context()
        {
            Target.Delete(PostId);

            Context.BlogPosts.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_Back_To_Index()
        {
            var actionResult = Target.Delete(PostId) as RedirectToRouteResult;

            actionResult.ShouldHaveRouteAction("Index");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Delete_The_Card_From_The_Trello_Service()
        {
            Service.Arrange(s => s.DeleteCard(Post.TrelloId));

            Target.Delete(PostId);

            Service.Assert(s => s.DeleteCard(Post.TrelloId), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Delete_Draft_From_Wordpress()
        {
            WordpressService.Arrange(ws => ws.DeleteFromWordpress(Arg.IsAny<BlogPost>()));

            Target.Delete(PostId);

            WordpressService.Assert(ws => ws.DeleteFromWordpress(Post));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Authorize_In_Wordpress()
        {
            WordpressService.Arrange(ws => ws.AuthorizeUser(Arg.AnyString));

            Target.Delete(PostId);

            WordpressService.Assert(ws => ws.AuthorizeUser(Arg.AnyString));
        }
}
}
