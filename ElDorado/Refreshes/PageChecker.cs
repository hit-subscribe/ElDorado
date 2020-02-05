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

        public bool IsPossiblyOutdated(string rawPageHtml)
        {
            var rawHtml = rawPageHtml.AsHtml();

            var title = GetPageTitle(rawHtml);
            var levelOneHeadings = GetLevelOneHeadings(rawHtml);

            return ContainsNonCurrentYear(title) || levelOneHeadings.Any(loh => ContainsNonCurrentYear(loh));
        }

        public bool ContainsNonCurrentYear(string targetText)
        {
            var tokensInText = targetText.Split(' ');
            var yearsInText = tokensInText.Where(t => Regex.IsMatch(t, @"^(20)\d{2}$"));

            return yearsInText.Any(y => int.Parse(y) < Now.Year);
        }

        public string GetPageTitle(HtmlNode page)
        {
            var allTitleNodes = page.SelectNodesWithTag("title");
            return allTitleNodes.Any() ? allTitleNodes.First().InnerText.ToString() : string.Empty;
        }

        public IEnumerable<string> GetLevelOneHeadings(HtmlNode page)
        {
            return page.SelectNodesWithTag("h1").Select(n => n.InnerText);
        }
    }
}
