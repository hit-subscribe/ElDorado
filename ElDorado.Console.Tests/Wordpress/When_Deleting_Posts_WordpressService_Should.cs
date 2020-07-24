using ElDorado.Console.Wordpress;
using ElDorado.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net.Http;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.Wordpress
{
    [TestClass]
    public class When_Deleting_Posts_WordpressService_Should
    {
        private const int PostWordpressId = 321;
        private const string PostTitle = "An Awesome Post";

        private BlogPost Post { get; set; } = new BlogPost()
        {
            Title = PostTitle,
            WordpressId = PostWordpressId,
        };

        private SimpleWebClient Client { get; set; } = Mock.Create<SimpleWebClient>();

        private WordpressService Target;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new WordpressService(Client);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Invoke_BearerRequest_Delete_On_Endpoint_Matching_Post_Id()
        {
            Target.DeleteFromWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Delete, $"{WordpressService.PostsEndpoint}/{PostWordpressId}", Arg.AnyString, Arg.AnyString), Occurs.Once());
        }
}
}
