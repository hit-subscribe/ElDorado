using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Metrics
{
    public class FeedlyInquisitor
    {
        private SimpleWebClient _client;

        public FeedlyInquisitor(SimpleWebClient client)
        {
            _client = client;
        }

        public virtual int GetSubscriberCount(string feedUrl)
        {
            var rawText = _client.GetRawResultOfBasicGetRequest(feedUrl);

            var feedlyFeed = JsonConvert.DeserializeObject<FeedlyFeed>(rawText);
            return feedlyFeed == null ? 0 : feedlyFeed.Subscribers;
        }
    }
}
