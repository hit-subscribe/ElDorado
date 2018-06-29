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

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle };

        private BlogPostsController Target { get; set; }


        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.BlogPosts.Add(Post);

            Target = new BlogPostsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Should_Save_The_Context_To_Capture_Delete()
        {
            bool wasCalled = false;
            Context.Arrange(ctx => ctx.SaveChanges()).DoInstead(() => wasCalled = true);

            Target.Delete(PostId);

            wasCalled.ShouldBe(true);
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

            actionResult.RouteValues["action"].ShouldBe("Index");
        }

}
}
