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
        private const string MasterSpreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";

        private readonly SheetsService _service;
        private readonly BlogPostFactory _factory = new BlogPostFactory();

        public PlanningSpreadsheetService()
        {
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetSheetsUserCredential(),
                ApplicationName = "El Dorado",
            });
        }

        public virtual IEnumerable<BlogPost> GetPlannedPosts(string range = "Current!A2:T")
        {
            var sheetCells = GetCells(range);

            return sheetCells.Where(r => IsSheetRowValid(r)).Select(r => _factory.MakePostFromGoogleSheetRow(r));
        }

        public void UpdatePostIds(IEnumerable<BlogPost> blogPosts, string range = "Current!A2:T")
        {
            var cells = GetCells(range);

            foreach (var row in cells)
            {
                var matchingBlogPost = blogPosts.FirstOrDefault(p => p.Title == row[1].ToString());
                row[19] = matchingBlogPost?.Id;
            }

            UpdateSpreadsheet(range, cells);
        }

        private IList<IList<object>> GetCells(string range)
        {
            var request = _service.Spreadsheets.Values.Get(MasterSpreadsheetId, range);
            return request.Execute().Values.Pad();
        }

        private void UpdateSpreadsheet(string range, IList<IList<object>> cells)
        {
            var updateRequest = _service.Spreadsheets.Values.Update(new ValueRange() { Values = cells }, MasterSpreadsheetId, range);
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
