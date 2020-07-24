using ElDorado.Console.Wordpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Net.Http;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.Wordpress
{
    [TestClass]
    public class When_Retrieving_Post_Drafts_WordpressService_Should
    {
        private const int BlogPostId = 123;
        private const string RawServerBlogPostText = "This ain’t an ‘awesome’ &#8220;blog post.&#8221;";
        private static readonly string RawServerPostJsonResponse = $"{{\"content\":{{\"rendered\":\"{RawServerBlogPostText}\",\"protected\":false}}}}";

        private SimpleWebClient Client { get; set; } = Mock.Create<SimpleWebClient>();

        private WordpressService Target;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Get, $"https://www.hitsubscribe.com/wp-json/wp/v2/posts/{BlogPostId}", Arg.AnyString, Arg.AnyString)).Returns(RawServerPostJsonResponse);
            Client.Arrange(cl => cl.GetRawResultOfBasicPostRequest(Arg.AnyString)).Returns("{token:\"asdf\"}");

            Target = new WordpressService(Client);
            Target.AuthorizeUser("username", "password");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_BlogPost_With_Content_Set()
        {
            var postContents = Target.GetBlogPostById(BlogPostId).Content;

            postContents.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_An_Exception_When_Authorization_Has_Not_Occurred()
        {
            var retriever = new WordpressService(Client);

            Should.Throw<WordpressAuthorizationException>(() => retriever.GetBlogPostById(1234));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Use_The_Bearer_Token_To_Invoke_The_Get_Request()
        {
            Target.GetBlogPostById(BlogPostId);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Get, Arg.AnyString, Target.Token, Arg.AnyString), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Decode_The_Html()
        {
            var postContents = Target.GetBlogPostById(BlogPostId).Content;

            postContents.ShouldBe("This ain't an 'awesome' \"blog post.\"");
        }
}
}
