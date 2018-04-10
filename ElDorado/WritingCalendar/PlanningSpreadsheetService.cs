using ElDorado.Domain;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElDorado.WritingCalendar
{
    public class PlanningSpreadsheetService
    {
        public virtual IEnumerable<BlogPost> GetPlannedPosts(string range = "Current!A2:T")
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetSheetsUserCredential(),
                ApplicationName = "El Dorado",
            });

            var spreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            var values = request.Execute().Values.Pad();

            var factory = new BlogPostFactory();

            return values.Where(r => IsSheetRowValid(r)).Select(r => factory.MakePostFromGoogleSheetRow(r));
        }

        public void UpdatePostIds(IEnumerable<BlogPost> blogPosts, string range = "Current!A2:T")
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetSheetsUserCredential(),
                ApplicationName = "El Dorado",
            });

            var spreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            var values = request.Execute().Values.Pad();

            var valueRange = new ValueRange();
            valueRange.Values = values;

            foreach(var row in values)
            {
                var matchingBlogPost = blogPosts.FirstOrDefault(p => p.Title == row[1].ToString());
                row[19] = matchingBlogPost?.Id;
            }

            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            updateRequest.Execute();
        }

        private static bool IsSheetRowValid(IList<object> r)
        {
            //We can pull this out for testing also, perhaps to the factory
            return r.Count > 0 && !string.IsNullOrEmpty(r[0].ToString()) && !string.IsNullOrEmpty(r[6].ToString());
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

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, new string[] { SheetsService.Scope.Spreadsheets }, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }

            return credential;
        }
    }
}
