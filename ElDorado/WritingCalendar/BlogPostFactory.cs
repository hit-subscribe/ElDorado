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
            if (googleSheetRow.Count < 18)
                throw new ArgumentException(nameof(googleSheetRow));

            return new BlogPost()
            {
                Blog = new Blog()
                {
                    CompanyName = googleSheetRow[0].ToString()
                },
                Title = googleSheetRow[1].ToString(),
                DraftDate = DateTime.Parse(googleSheetRow[6].ToString()),
                IsApproved = googleSheetRow[17].ToString() == "Yes"
            };
        }
    }
}
