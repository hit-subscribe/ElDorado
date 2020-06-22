using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Refreshes
{
    //This is pretty clunky, since I bolted the idea of showing the blog post title on after the fact
    //I'm thinking I might clean this up a bit by creating an audit entry type or something that contains the title and any problems
    public class AuditResult
    {
        private Dictionary<string, List<string>> _problems = new Dictionary<string, List<string>>();
        private Dictionary<string, Page> _pages = new Dictionary<string, Page>();

        public void AddProblem(Page page, string problem)
        {
            if (!_problems.ContainsKey(page.Url))
                _problems[page.Url] = new List<string>();

            if (!_pages.ContainsKey(page.Url))
                _pages[page.Url] = page;

            _problems[page.Url].Add(problem);
        }

        public string ToCsv()
        {
            var fileContents = new StringBuilder();

            foreach (var kvp in _problems)
                fileContents.AppendLine($"{kvp.Key},{_pages[kvp.Key].Title},{String.Join(",", kvp.Value)}");

            return fileContents.ToString();
        }
    }
}
