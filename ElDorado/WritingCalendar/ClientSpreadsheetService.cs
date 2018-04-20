using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.WritingCalendar
{
    public class ClientSpreadsheetService
    {
        private readonly GoogleSheet _clientSheet;
        public ClientSpreadsheetService(GoogleSheet googleSheet)
        {
            _clientSheet = googleSheet;
        }

        public void AddPosts(IEnumerable<BlogPost> posts)
        {
            IList<IList<object>> rows = new List<IList<object>>();

            foreach (var post in posts)
            {
                IList<object> columnValues = new List<object>() { post.Title, string.Empty, string.Empty, post.Mission, post.TargetPublicationDate };
                rows.Add(columnValues);
            }

            _clientSheet.UpdateSpreadsheet("asdf", rows);
        }
    }
}
