using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Refreshes
{
    public class AuditResult
    {
        private readonly List<PageCheckResult> _pageCheckResults = new List<PageCheckResult>();

        public void AddPageCheckResult(PageCheckResult result)
        {
            _pageCheckResults.Add(result);
        }

        public string ProblemsToCsv()
        {
            var fileContents = new StringBuilder();

            foreach (var result in _pageCheckResults.Where(pcr => pcr.Issues.Any()))
                fileContents.AppendLine($"{result.PageUrl},\"{result.PageTitle}\",{String.Join(",", result.Issues)}");

            return fileContents.ToString();
        }
    }
}
