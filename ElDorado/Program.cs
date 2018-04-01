using ElDorado.Domain;
using ElDorado.Metrics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
            //ReadFromGoogleSheet();
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

        private static void ReadFromGoogleSheet()
        {
            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetSheetsUserCredential(),
                ApplicationName = "El Dorado",
            });

            var spreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";
            var range = "Current!A2:E";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            var values = request.Execute().Values;
            Pad(values);

            Console.WriteLine("Client, Title");
            foreach (var row in values.Where(r => r.Count > 0))
                Console.WriteLine("{0}, {1}", row[0], row[1]);

            Console.Read();
        }

        private static UserCredential GetSheetsUserCredential()
        {
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
            UserCredential credential;

            using (var stream = new FileStream(@"CredFiles\google.cred", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, new string[] { SheetsService.Scope.SpreadsheetsReadonly }, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }

        private static void Pad(IList<IList<object>> target)
        {
            var maxColumns = target.Max(r => r.Count);
            foreach(var row in target)
            {
                for (int i = row.Count; i < maxColumns; i++)
                    row.Add(string.Empty);
            }
        }
    }
}
