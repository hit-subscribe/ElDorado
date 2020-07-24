using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Refreshes
{
    public class PageCheckResult
    {
        private readonly List<string> _issues = new List<string>();

        public string PageTitle { get; set; }

        public string PageUrl { get; set; }

        public IEnumerable<string> Issues => _issues;

        public PageCheckResult() : this(null, null) { }

        public PageCheckResult(string url, string title) : this(url, title, new string[0]) { }

        public PageCheckResult(string url, string title, params string[] issues)
        {
            PageUrl = url;
            PageTitle = title;
            _issues.AddRange(issues);
        }

        public void AddIssue(string issue)
        {
            _issues.Add(issue);
        }

        public void AddIssues(IEnumerable<string> issues)
        {
            foreach (var issue in issues)
                AddIssue(issue);
        }
    }
}
