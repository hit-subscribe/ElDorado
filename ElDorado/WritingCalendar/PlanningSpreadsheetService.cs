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
            var rows = _plannedPostsSheet.GetCells(range).Pad(20).Select(r => new GoogleSheetRow(r));

            return rows.Where(r => IsSheetRowValid(r)).Select(r => MakePostFromGoogleSheetRow(r));
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
        private BlogPost MakePostFromGoogleSheetRow(GoogleSheetRow row)
        {
            return new BlogPost()
            {
                Blog = new Blog() { CompanyName = row.Item(0) },
                Title = row.Item(1),
                Mission = row.Item(4),
                Author = new Author() { FirstName = row.Item(5) },
                DraftDate = row.ItemAsDate(6),
                TargetFinalizeDate = row.ItemAsDate(7),
                TargetPublicationDate = row.ItemAsDate(8),
                Keyword = row.Item(9),
                IsApproved = row.ItemAsBool(17),
                IsDoublePost = row.ItemAsBool(18),
                Id = row.ItemAsInt(19)
            };
        }
        private static bool IsSheetRowValid(GoogleSheetRow row)
        {
            return row.Count > 0 && 
                !string.IsNullOrEmpty(row.Item(0)) && 
                !string.IsNullOrEmpty(row.Item(1)) &&
                !string.IsNullOrEmpty(row.Item(6));
        }
    }
}
