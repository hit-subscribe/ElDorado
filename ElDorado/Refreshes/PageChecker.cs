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

        public AuditResult AuditSiteFromSiteMap(Sitemap siteMap)
        {
            var result = new AuditResult();

            foreach(var siteUrl in siteMap.SiteUrls)
            {
                var page = new Page(_client.GetRawResultOfBasicGetRequest(siteUrl.Url), siteUrl.Url);

                if (IsPossiblyOutdated(page))
                    result.AddProblem(page, "Is possibly outdated");

                var problemWords = GetProblematicWords(page).ToList();
                if (problemWords.Any())
                    problemWords.ForEach(pw => result.AddProblem(page, $"Contains term \"{pw}\""));
            }

            return result;
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
