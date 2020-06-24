using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElDorado.Refreshes
{
    public class PageChecker
    {
        private readonly SimpleWebClient _client;
        public DateTime Now { get; set; } = DateTime.Now;
        
        public List<string> ProblemTerms { get; set; } = new List<string>();

        public PageChecker(SimpleWebClient client = null)
        {
            _client = client ?? new SimpleWebClient();
        }

        public async Task<AuditResult> AuditSiteFromSiteMap(Sitemap siteMap)
        {
            var result = new AuditResult();

            var pageResultTasks = new List<Task<PageCheckResult>>();

            foreach(var pageUrl in siteMap.SiteUrls.Select(su => su.Url))
            {
                pageResultTasks.Add(GetPageResult(pageUrl));
                //var pageResult = GetPageResult(pageUrl);
                //result.AddPageCheckResult(pageResult);
            }
            await Task.WhenAll(pageResultTasks);

            foreach (var taskResult in pageResultTasks.Select(prt => prt.Result))
                result.AddPageCheckResult(taskResult);

            return result;
        }

        private async Task<PageCheckResult> GetPageResult(string pageUrl)
        {
            string rawHtml = await _client.GetRawResultOfBasicGetRequestAsync(pageUrl);

            var page = new Page(rawHtml, pageUrl);
            var pageResult = new PageCheckResult() { PageUrl = pageUrl, PageTitle = page.Title };

            if (IsPossiblyOutdated(page))
                pageResult.AddIssue("Is possibly outdated");

            var problemWords = GetProblematicWords(page).ToList();
            if (problemWords.Any())
                problemWords.ForEach(pw => pageResult.AddIssue($"Contains term \"{pw}\""));

            return pageResult;
        }

        public IEnumerable<string> GetLinksFrom(string rawPageHtml)
        {
            return rawPageHtml.AsHtml().SelectAttributeValuesForNode("href", "a").Where(link => IsActualLink(link));
        }

        private bool IsPossiblyOutdated(Page rawPage)
        {
            return ContainsNonCurrentYear(rawPage.Title) || rawPage.H1s.Any(h1 => ContainsNonCurrentYear(h1));
        }

        private IEnumerable<string> GetProblematicWords(Page rawPage)
        {
            var pageBodyWithNoUrls = Regex.Replace(rawPage.Body, "href=\"*\"", string.Empty);

            return ProblemTerms.Where(pt => Regex.IsMatch(pageBodyWithNoUrls, $@"\b{pt}\b", RegexOptions.IgnoreCase));
        }

        private bool ContainsNonCurrentYear(string targetText)
        {
            var tokensInText = targetText.Split(' ');
            var yearsInText = tokensInText.Where(t => Regex.IsMatch(t, @"^(20)\d{2}$"));

            return yearsInText.Any(y => int.Parse(y) < Now.Year);
        }

        private static bool IsActualLink(string hrefAttribute)
        {
            return hrefAttribute != null && !hrefAttribute.StartsWith("#");
        }
    }
}
