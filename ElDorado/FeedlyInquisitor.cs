using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    public class FeedlyInquisitor
    {
        private SimpleWebClient _client;

        public FeedlyInquisitor(SimpleWebClient client)
        {
            _client = client;
        }

        public int GetSubscriberCount(string feedUrl)
        {
            var rawText = _client.GetRawText(feedUrl);

            var feedlyFeed = JsonConvert.DeserializeObject<FeedlyFeed>(rawText);
            return feedlyFeed == null ? 0 : feedlyFeed.Subscribers;
        }
    }
}
