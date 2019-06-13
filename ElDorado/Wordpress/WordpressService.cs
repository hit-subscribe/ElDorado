using ElDorado.Domain;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ElDorado.Wordpress
{
    public class WordpressService
    {
        private readonly SimpleWebClient _client;

        public const string BaseSite = "https://www.hitsubscribe.com/";
        public static readonly string PostsEndpoint = $"{BaseSite}wp-json/wp/v2/posts";
        public static readonly string AuthEndpoint = $"{BaseSite}wp-json/jwt-auth/v1/token";

        public string Token { get; private set; }

        public WordpressService(SimpleWebClient client)
        {
            _client = client;
        }

        public virtual void AuthorizeUser(string filePath)
        {
            var credentialStore = new CredentialStore(File.ReadAllText(filePath));
            AuthorizeUser(credentialStore["Username"], credentialStore["Password"]);
        }

        public virtual void AuthorizeUser(string username, string password)
        {
            try
            {
                var rawJson = _client.GetRawResultOfBasicPostRequest($"{AuthEndpoint}?username={username}&password={WebUtility.UrlEncode(password)}");
                dynamic wordpressCredential = JsonConvert.DeserializeObject(rawJson);
                Token = wordpressCredential.token;
            }
            catch (Exception ex)
            {
                throw new WordpressAuthorizationException(ex);
            }
        }

        public virtual BlogPost GetBlogPostById(int wordpressId)
        {
            if (string.IsNullOrEmpty(Token))
                throw new WordpressAuthorizationException("You have to authorize a user to invoke this.");

            var blogPostJson = _client.GetRawResultOfBearerRequest(HttpMethod.Get, $"{PostsEndpoint}/{wordpressId}", Token);
            dynamic blogPostContents = JsonConvert.DeserializeObject(blogPostJson);
            return new BlogPost()
            {
                Content = blogPostContents.content.rendered
            };
        }

        public virtual void SyncToWordpress(BlogPost post)
        {
            bool areWeCreatingANewPost = post.WordpressId == 0;

            if (areWeCreatingANewPost)
                CreatePostInWordpress(post);
            else
                EditPostInWordpress(post);

        }

        public virtual void DeleteFromWordpress(BlogPost post)
        {
            _client.GetRawResultOfBearerRequest(HttpMethod.Delete, $"{PostsEndpoint}/{post.WordpressId}", Token);
        }


        //This is a lot of private cruft, Erik, maybe a new class is in order here

        private void EditPostInWordpress(BlogPost post)
        {
            var postToMakeJson = new
            {
                author = post.Author?.WordpressId ?? 0
            };

            dynamic wordpressPost = SendPostToWordpressAndGetResultantJson(post, $"{PostsEndpoint}/{post.WordpressId}", postToMakeJson.ToJsonString());
            ValidateAUthorMatchOrDieTrying(post, wordpressPost);
        }

        private void CreatePostInWordpress(BlogPost post)
        {
            var postToMakeJson = new
            {
                title = post.Title,
                author = post.Author?.WordpressId ?? 0
            };

            dynamic wordpressPost = SendPostToWordpressAndGetResultantJson(post, PostsEndpoint, postToMakeJson.ToJsonString());
            post.WordpressId = wordpressPost.id;

            ValidateAUthorMatchOrDieTrying(post, wordpressPost);
        }

        private static void ValidateAUthorMatchOrDieTrying(BlogPost post, dynamic wordpressPost)
        {
            if (post.Author != null && post.Author.WordpressId != (int)wordpressPost.author)
                throw new MissingAuthorException();
        }

        private dynamic SendPostToWordpressAndGetResultantJson(BlogPost post, string endpoint, string json)
        {
            var rawJson = _client.GetRawResultOfBearerRequest(HttpMethod.Post, endpoint, Token, json);

            return JsonConvert.DeserializeObject(rawJson);
        }
    }
}
