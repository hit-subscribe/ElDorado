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
        public DateTime Now { get; set; } = DateTime.Now;
        
        public List<string> ProblemTerms { get; set; } = new List<string>();

        public bool IsPossiblyOutdated(string rawPageHtml)
        {
            var rawHtml = rawPageHtml.AsHtml();

            var title = GetPageTitle(rawHtml);
            var levelOneHeadings = GetLevelOneHeadings(rawHtml);

            return ContainsNonCurrentYear(title) || levelOneHeadings.Any(loh => ContainsNonCurrentYear(loh));
        }

        public IEnumerable<string> GetLinksFrom(string rawPageHtml)
        {
            return rawPageHtml.AsHtml().SelectAttributeValuesForNode("href", "a").Where(link => IsActualLink(link));
        }

        public IEnumerable<string> GetProblematicWords(string rawPageHtml)
        {
            var pageBody = rawPageHtml.AsHtml().SelectNodesWithTag("body").FirstOrDefault()?.InnerText?.ToString() ?? string.Empty;
            var pageBodyWithNoUrls = Regex.Replace(pageBody, "href=\"*\"", string.Empty);

            return ProblemTerms.Where(pt => Regex.IsMatch(pageBodyWithNoUrls, $@"\b{pt}\b", RegexOptions.IgnoreCase));
        }

        private bool ContainsNonCurrentYear(string targetText)
        {
            var tokensInText = targetText.Split(' ');
            var yearsInText = tokensInText.Where(t => Regex.IsMatch(t, @"^(20)\d{2}$"));

            return yearsInText.Any(y => int.Parse(y) < Now.Year);
        }

        private string GetPageTitle(HtmlNode page)
        {
            var allTitleNodes = page.SelectNodesWithTag("title");
            return allTitleNodes.Any() ? allTitleNodes.First().InnerText.ToString() : string.Empty;
        }

        private IEnumerable<string> GetLevelOneHeadings(HtmlNode page)
        {
            return page.SelectNodesWithTag("h1").Select(n => n.InnerText);
        }

        private static bool IsActualLink(string hrefAttribute)
        {
            return hrefAttribute != null && !hrefAttribute.StartsWith("#");
        }
    }
}
