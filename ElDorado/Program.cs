using ElDorado.Domain;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElDorado
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateBlogMetrics();

            //SetupTrelloConnection();
            //AddCardToTrello();
        }

        private static void GenerateBlogMetrics()
        {
            var webClient = new SimpleWebClient();
            var feedlyInquisitor = new FeedlyInquisitor(webClient);
            var alexaInquisitor = new AlexaDataInquisitor(webClient);
            var mozInquisitor = new MozInquisitor(new CredentialStore(File.ReadAllText(@"CredFiles\moz.cred")), webClient) { Timeout = 10 };

            var metricsRecorder = new BlogMetricsRecorder(feedlyInquisitor, alexaInquisitor, mozInquisitor);

            using (var context = new BlogContext())
            {
                var blogs = context.Blogs.ToList();
                var blogMetrics = metricsRecorder.GenerateMetrics(blogs);
                context.BlogMetrics.AddRange(blogMetrics);
                context.SaveChanges();
            }
        }

        private static void SetupTrelloConnection()
        {
            var credentials = new CredentialStore(File.ReadAllText(@"CredFiles\trello.cred"));
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
            TrelloAuthorization.Default.AppKey = credentials["TrelloAppKey"];
            TrelloAuthorization.Default.UserToken = credentials["TrelloUserToken"]; 
        }

        private static void AddCardToTrello()
        {
            var board = new Board("AhqnpUJD");
            var plannedPostsList = board.Lists.First(l => l.Name == "Planned Posts");
            plannedPostsList.Cards.Add("A fifth added from El Dorado.");
        }
    }
}
