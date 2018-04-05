using ElDorado.Domain;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElDorado.WritingCalendar
{
    public class PlanningSpreadsheetService
    {
        public virtual IEnumerable<BlogPost> GetPlannedPosts()
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetSheetsUserCredential(),
                ApplicationName = "El Dorado",
            });

            var spreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";
            var range = "Current!A2:S";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            var values = request.Execute().Values.Pad();

            var factory = new BlogPostFactory();

            return values.Where(r => r.Count > 0 && !string.IsNullOrEmpty(r[0].ToString())).Select(r => factory.MakePostFromGoogleSheetRow(r));
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
    }
}
