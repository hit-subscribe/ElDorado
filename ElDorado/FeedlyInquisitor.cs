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
            if(!string.IsNullOrEmpty(rawText))
                return int.Parse(rawText.Split(',').First(s => s.Contains("subscribers")).Split(':')[1]);

            return 0;
        }
    }
}
