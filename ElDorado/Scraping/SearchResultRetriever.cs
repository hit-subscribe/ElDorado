using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Scraping
{
    public class SearchResultRetriever
    {
        public string BaseSearchQuery { get; private set; }

        private readonly SimpleWebClient _client;

        public SearchResultRetriever(SimpleWebClient simpleWebClient, CredentialStore store)
        {
            _client = simpleWebClient;
            BaseSearchQuery = $"https://www.googleapis.com/customsearch/v1?key={store["ApiKey"]}&cx={store["CseId"]}&q=";
        }

        public IEnumerable<SearchResult> SearchFor(string searchString)
        {
            var rawText = _client.GetRawText(BaseSearchQuery + searchString);

            try
            {
                var resultJson = JObject.Parse(rawText);
                var serpResults = resultJson["items"].Children();

                return serpResults.Select(sr => JsonConvert.DeserializeObject<SearchResult>(sr.ToString()));
            }
            catch
            {
                return Enumerable.Empty<SearchResult>();
            }
        }
    }
}
