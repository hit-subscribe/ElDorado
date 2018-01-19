using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    public class SimpleWebClient
    {
        public virtual string GetRawText(string feedUrl)
        {
            var client = new HttpClient();
            var result = client.GetAsync(feedUrl).Result;
            var results = result.Content.ReadAsStringAsync().Result;
            return results;
        }
    }
}
