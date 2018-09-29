using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Trello;
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
    public class When_Deleting_BlogPosts_BlogPostsController_Should
    {
        private const string PostTitle = "This One Weird Trick";
        private const int PostId = 4;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private TrelloWritingCalendarService Service { get; set; } = Mock.Create<TrelloWritingCalendarService>();

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.BlogPosts.Add(Post);

            Target = new BlogPostsController(Context, Service) { MapPath = "Doesn't matter" };
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

}
}
