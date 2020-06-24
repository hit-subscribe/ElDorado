using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Refreshes
{
    public class PageCheckResult
    {
        private readonly List<string> _issues = new List<string>();

        public string PageTitle { get; set; }

        public string PageUrl { get; set; }

        public IEnumerable<string> Issues => _issues;

        public void AddIssue(string issue)
        {
            _issues.Add(issue);
        }
    }
}
