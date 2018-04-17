using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.WritingCalendar
{
    public class BlogPostFactory
    {
        public BlogPost MakePostFromGoogleSheetRow(IList<object> googleSheetRow)
        {
            var sheetRowStrings = googleSheetRow.Select(o => o?.ToString()).ToList();
            if (googleSheetRow.Count < 18)
                throw new ArgumentException(nameof(googleSheetRow));

            return new BlogPost()
            {
                Blog = new Blog()
                {
                    CompanyName = sheetRowStrings[0]
                },
                Title = sheetRowStrings[1],
                Mission = sheetRowStrings[4],
                Author = sheetRowStrings[5],
                DraftDate = DateTime.Parse(sheetRowStrings[6]),
                TargetFinalizeDate = sheetRowStrings[7].SafeToDateTime(),
                TargetPublicationDate = sheetRowStrings[8].SafeToDateTime(),
                Keyword = sheetRowStrings[9],
                IsApproved = sheetRowStrings[17] == "Yes",
                IsDoublePost = sheetRowStrings[18] == "Yes"
            };
        }
    }
}
