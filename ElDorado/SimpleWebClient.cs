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
        HttpClient _basicClient = new HttpClient();

        public virtual string GetRawResultOfBasicGetRequest(string url)
        {
            return GetRawTextFrom(_basicClient.GetAsync(url));
        }

        public virtual string GetRawResultOfBearerGetRequest(string url, string bearerToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            return GetRawTextFrom(client.GetAsync(url));
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
