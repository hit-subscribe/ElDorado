using ElDorado.Domain;
using ElDorado.Wordpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.Wordpress
{
    [TestClass]
    public class When_Syncing_To_Wordpress_WordpressService_Should
    {
        private const int PostWordpressId = 12345;
        private const string PostTitle = "An Awesome Post";

        private SimpleWebClient Client { get; set; } = Mock.Create<SimpleWebClient>();

        private BlogPost Post { get; set; } = new BlogPost() { Title = PostTitle, WordpressId = PostWordpressId };

        private WordpressService Target;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, WordpressService.PostsEndpoint, Arg.AnyString, Arg.AnyString)).Returns($"{{\"id\":{PostWordpressId}}}");

            Target = new WordpressService(Client);   
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Create_A_New_Draft_When_Post_Has_No_Wordpress_Id()
        {
            Post.WordpressId = 0;

            Target.SyncToWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, WordpressService.PostsEndpoint, Arg.AnyString, $"{{\"title\":\"{PostTitle}\"}}"), Occurs.Once());

        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Existing_Draft_When_Post_Has_A_Wordpress_Id()
        {
            Target.SyncToWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, $"{WordpressService.PostsEndpoint}/{PostWordpressId}", Arg.AnyString, $"{{\"title\":\"{PostTitle}\"}}"));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Blog_Post_WordpressId_To_Returned_Id()
        {
            Post.WordpressId = 0;

            Target.SyncToWordpress(Post);

            Post.WordpressId.ShouldBe(PostWordpressId);
        }
}
}
