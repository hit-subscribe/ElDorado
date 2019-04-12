using ElDorado.Wordpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.Wordpress
{
    [TestClass]
    public class When_Retrieving_Post_Drafts_BlogPostRetriever_Should
    {
        private const int BlogPostId = 123;
        private const string RawServerBlogPost = "This is an awesome blog post.";
        private static readonly string RawServerPostJsonResponse = $"{{\"content\":{{\"rendered\":\"{RawServerBlogPost}\",\"protected\":false}}}}";

        private SimpleWebClient Client { get; set; } = Mock.Create<SimpleWebClient>();

        private BlogPostRetriever Target;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(cl => cl.GetRawResultOfBearerGetRequest($"https://daedtech.com/wp-json/wp/v2/posts/{BlogPostId}", Arg.AnyString)).Returns(RawServerPostJsonResponse);
            Client.Arrange(cl => cl.GetRawResultOfBasicPostRequest(Arg.AnyString)).Returns("{token:\"asdf\"}");

            Target = new BlogPostRetriever(Client);
            Target.AuthorizeUser("username", "password");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_String_Containing_The_Raw_Blog_Post_Content()
        {
            var postContents = Target.GetBlogPostById(BlogPostId);

            postContents.ShouldBe(RawServerBlogPost);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_An_Exception_When_Authorization_Has_Not_Occurred()
        {
            var retriever = new BlogPostRetriever(Client);

            Should.Throw<WordpressAuthorizationException>(() => retriever.GetBlogPostById(1234));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Use_The_Bearer_Token_To_Invoke_The_Get_Request()
        {
            Target.GetBlogPostById(BlogPostId);

            Client.Assert(cl => cl.GetRawResultOfBearerGetRequest(Arg.AnyString, Target.Token), Occurs.Once());
        }

    }
}
