using ElDorado.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    public class SimpleWebClient
    {
        private HttpClient _basicClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(30) };

        public virtual string GetRawResultOfBasicGetRequest(string url)
        {
            return GetRawTextFrom(_basicClient.GetAsync(url));
        }

        public async virtual Task<string> GetRawResultOfBasicGetRequestAsync(string url)
        {
            var message = await _basicClient.GetAsync(url);

            return message.Content.ReadAsStringAsync().Result;
        }

        public virtual string GetRawResultOfBearerRequest(HttpMethod method, string url, string bearerToken, string content = null)
        {
            var client = new HttpClient();

            using (var requestMessage = new HttpRequestMessage(method, url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                requestMessage.Headers.Add("user-agent", "El Dorado");
                if (content != null)
                {
                    requestMessage.Content = new StringContent(content);
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                return GetRawTextFrom(client.SendAsync(requestMessage));
            }
        }

        public virtual string GetRawResultOfBasicPostRequest(string url)
        {
            return GetRawTextFrom(_basicClient.PostAsync(url, null));
        }

        private string GetRawTextFrom(Task<HttpResponseMessage> message)
        {
            var result = message.Result;
            return result.Content.ReadAsStringAsync().Result;
        }
    }
}
