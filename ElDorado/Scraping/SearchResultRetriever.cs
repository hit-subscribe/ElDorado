using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElDorado.Scraping
{
    public class SearchResultRetriever
    {
        private readonly SimpleWebClient _client;

        public string BaseSearchQuery { get; private set; }

        public Action<int> Delay { get; set; } = i => Thread.Sleep(1000 * i);

        public SearchResultRetriever(SimpleWebClient simpleWebClient, CredentialStore store)
        {
            _client = simpleWebClient;
            BaseSearchQuery = $"https://www.googleapis.com/customsearch/v1?key={store["ApiKey"]}&cx={store["CseId"]}&q=";
        }

        public IEnumerable<SearchResult> SearchFor(string searchTerm, int serpPages = 1)
        {
            var allResults = new List<SearchResult>();
            AddResultsToList(searchTerm, serpPages, allResults);
            return allResults;
        }

        private void AddResultsToList(string searchTerm, int serpPages, List<SearchResult> allResults)
        {
            for (int resultNumber = 0; resultNumber < serpPages; resultNumber++)
            {
                int startingResultNumber = resultNumber * 10 + 1;
                string searchParameters = $"{searchTerm}&start={startingResultNumber}";
                var singlePageResults = ExecuteSearch(searchParameters);
                allResults.AddRange(singlePageResults);

                if(resultNumber + 1 < serpPages && singlePageResults.Any())
                    Delay(30);
            }
        }

        private IEnumerable<SearchResult> ExecuteSearch(string searchParameters)
        {
            var rawText = _client.GetRawResultOfBasicGetRequest(BaseSearchQuery + searchParameters);

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
