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
        private const int AuthorWordpressId = 12;
        private const int PostWordpressId = 12345;
        private const string PostTitle = "An Awesome Post";

        private SimpleWebClient Client { get; set; } = Mock.Create<SimpleWebClient>();

        private BlogPost Post { get; set; } = new BlogPost()
        {
            Title = PostTitle,
            WordpressId = PostWordpressId,
            Author = new Author() { WordpressId = AuthorWordpressId }
        };

        private WordpressService Target;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, WordpressService.PostsEndpoint, Arg.AnyString, Arg.AnyString)).Returns($"{{\"id\":{PostWordpressId}, \"author\":{AuthorWordpressId}}}");

            Target = new WordpressService(Client);   
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Create_A_New_Draft_When_Post_Has_No_Wordpress_Id()
        {
            Post.WordpressId = 0;

            Target.SyncToWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, WordpressService.PostsEndpoint, Arg.AnyString, Arg.AnyString), Occurs.Once());

        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Existing_Draft_When_Post_Has_A_Wordpress_Id()
        {
            Target.SyncToWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, $"{WordpressService.PostsEndpoint}/{PostWordpressId}", Arg.AnyString, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Blog_Post_WordpressId_To_Returned_Id()
        {
            Post.WordpressId = 0;

            Target.SyncToWordpress(Post);

            Post.WordpressId.ShouldBe(PostWordpressId);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_Author_To_BlogPost_Authors_WordpressId()
        {
            Target.SyncToWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, $"{WordpressService.PostsEndpoint}/{PostWordpressId}", Arg.AnyString, Arg.Matches<string>(s => s.Contains($"\"author\":{AuthorWordpressId}"))));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_AuthorId_To_Zero_When_Has_No_Author()
        {
            Post.Author = null;

            Target.SyncToWordpress(Post);

            Client.Assert(cl => cl.GetRawResultOfBearerRequest(HttpMethod.Post, $"{WordpressService.PostsEndpoint}/{PostWordpressId}", Arg.AnyString, Arg.Matches<string>(s => s.Contains($"\"author\":0"))));
        }

    [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_An_Exception_When_Post_Author_Wordpress_Id_Does_Not_Match_Json_Author_Id()
        {
            Post.Author.WordpressId = 16;
            Post.WordpressId = 0;

            Should.Throw<MissingAuthorException>(() => Target.SyncToWordpress(Post));
        }
}
}
