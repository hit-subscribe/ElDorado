using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.WritingCalendar
{
    public class PlanningSpreadsheetService
    {
        private readonly GoogleSheet _plannedPostsSheet;

        public PlanningSpreadsheetService(GoogleSheet plannedPostsSheet)
        {
            _plannedPostsSheet = plannedPostsSheet;
        }

        public virtual IEnumerable<BlogPost> GetPosts(string range = "Current!A2:T")
        {
            var sheetCells = _plannedPostsSheet.GetCells(range).Pad(20);

            return sheetCells.Where(r => IsSheetRowValid(r)).Select(r => MakePostFromGoogleSheetRow(r));
        }

        public void UpdatePostIds(IEnumerable<BlogPost> blogPosts, string range = "Current!A2:T")
        {
            var cells = _plannedPostsSheet.GetCells(range).Pad(20);

            foreach (var row in cells)
            {
                var matchingBlogPost = blogPosts.FirstOrDefault(p => p.Title == row[1].ToString());
                row[19] = matchingBlogPost?.Id;
            }

            _plannedPostsSheet.UpdateSpreadsheet(range, cells);
        }
        private BlogPost MakePostFromGoogleSheetRow(IList<object> googleSheetRow)
        {
            var sheetRowStrings = googleSheetRow.Select(o => o?.ToString()).ToList();

            return new BlogPost()
            {
                Blog = new Blog() { CompanyName = sheetRowStrings[0] },
                Title = sheetRowStrings[1],
                Mission = sheetRowStrings[4],
                Author = new Author() { FirstName = sheetRowStrings[5] },
                DraftDate = DateTime.Parse(sheetRowStrings[6]),
                TargetFinalizeDate = sheetRowStrings[7].SafeToDateTime(),
                TargetPublicationDate = sheetRowStrings[8].SafeToDateTime(),
                Keyword = sheetRowStrings[9],
                IsApproved = sheetRowStrings[17] == "Yes",
                IsDoublePost = sheetRowStrings[18] == "Yes",
                Id = string.IsNullOrEmpty(sheetRowStrings[19]) ? 0 : int.Parse(sheetRowStrings[19])
            };
        }
        private static bool IsSheetRowValid(IList<object> r)
        {
            return r.Count > 0 && 
                !string.IsNullOrEmpty(r[0].ToString()) && 
                !string.IsNullOrEmpty(r[1].ToString()) &&
                !string.IsNullOrEmpty(r[6].ToString());
        }
    }
}
