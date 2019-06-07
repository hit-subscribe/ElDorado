using ElDorado.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            catch(Exception ex)
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
            var postJson = BuildJsonBodyFromPost(post);
            var url = post.WordpressId == 0 ? PostsEndpoint : $"{PostsEndpoint}/{post.WordpressId}";

            var rawJson = _client.GetRawResultOfBearerRequest(HttpMethod.Post, url, Token, postJson);
            SetWordpressIdIfNeeded(post, rawJson);
        }

        public virtual void DeleteFromWordpress(BlogPost post)
        {
            _client.GetRawResultOfBearerRequest(HttpMethod.Delete, $"{PostsEndpoint}/{post.WordpressId}", Token);
        }

        private static void SetWordpressIdIfNeeded(BlogPost post, string rawJson)
        {
            if (post.WordpressId == 0)
            {
                dynamic wordpressPost = JsonConvert.DeserializeObject(rawJson);
                post.WordpressId = wordpressPost.id;
                if (post.Author.WordpressId != (int)wordpressPost.author)
                    throw new MissingAuthorException();
            }
        }

        private string BuildJsonBodyFromPost(BlogPost post)
        {
            return $"{{\"title\":\"{post.Title}\", \"author\":{post.Author?.WordpressId ?? 0}}}";
        }
    }
}
