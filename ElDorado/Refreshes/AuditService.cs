using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElDorado.Console.Refreshes
{
    public class AuditService
    {
        private readonly SimpleWebClient _client;

        public DateTime Now { get; set; } = DateTime.Now;

        public AuditService(SimpleWebClient client)
        {
            _client = client;
        }

        public virtual async Task<IEnumerable<PageCheckResult>> GetPageResultsAsync(IEnumerable<string> urls)
        {
            var results = new List<PageCheckResult>();
            foreach(var url in urls)
            {
                var result = await GetPageResultAsync(url);
                results.Add(result);
            }
            return results;
        }
        public async Task<PageCheckResult> GetPageResultAsync(string url)
        {
            var rawHtml = await _client.GetRawResultOfBasicGetRequestAsync(url);

            var page = new Page(rawHtml, url);
            var pageResult = new PageCheckResult(url, page.Title);

            CheckForOutdated(page, pageResult);

            return pageResult;
        }

        private void CheckForOutdated(Page page, PageCheckResult pageResult)
        {
            if (ContainsNonCurrentYear(page.Title) || page.H1s.Any(h1 => ContainsNonCurrentYear(h1)))
                pageResult.AddIssue("Is possibly outdated");
        }

        private bool ContainsNonCurrentYear(string targetText)
        {
            var tokensInText = targetText.Split(' ');
            var yearsInText = tokensInText.Where(t => Regex.IsMatch(t, @"^(20)\d{2}$"));

            return yearsInText.Any(y => int.Parse(y) < Now.Year);
        }
    }
}
