using ElDorado.Domain;
using ElDorado.Gui.Controllers;
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
        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private BlogPostsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogPostsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_View_Containing_The_BlogPosts_From_Context()
        {
            const string title = "A Thought Provoking Piece";

            Context.BlogPosts.Add(new BlogPost() { Title = title });

            var posts = Target.Index().GetViewResultModel<IEnumerable<BlogPost>>();

            posts.First().Title.ShouldBe(title);
        }
    }
}
