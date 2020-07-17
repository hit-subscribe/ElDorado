using ElDorado.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ElDorado.Refreshes
{
    public class PageChecker
    {
        private static readonly SemaphoreSlim _throttler = new SemaphoreSlim(500);

        private readonly SimpleWebClient _client;
        public DateTime Now { get; set; } = DateTime.Now;
        
        public List<string> ProblemTerms { get; set; } = new List<string>();

        public PageChecker(SimpleWebClient client = null)
        {
            _client = client ?? new SimpleWebClient();
        }

        public async Task<AuditResult> AuditSiteFromSiteMap(Sitemap siteMap)
        {
            var pageResultTasks = new List<Task<PageCheckResult>>();

            siteMap.Urls.ToList().ForEach(su => pageResultTasks.Add(GetPageResult(su)));

            await Task.WhenAll(pageResultTasks);

            var result = new AuditResult();
            result.AddPageCheckResults(pageResultTasks.Select(prt => prt.Result));

            return result;
        }

        private async Task<PageCheckResult> GetPageResult(string pageUrl)
        {
            await _throttler.WaitAsync();

            try
            {
                string rawHtml = await _client.GetRawResultOfBasicGetRequestAsync(pageUrl);

                var page = new Page(rawHtml, pageUrl);
                var pageResult = new PageCheckResult(pageUrl, page.Title);

                CheckForOutdated(page, pageResult);
                CheckForProblematicWords(page, pageResult);
                await CheckForBadLinks(page.InPostExternalLinks, pageResult);

                return pageResult;
            }
            catch(Exception ex)
            {
                return new PageCheckResult(pageUrl, string.Empty, "This page generated an exception on parsing.");
            }
            finally
            {
                _throttler.Release();
            }
        }

        private async Task CheckForBadLinks(IEnumerable<Link> inPostExternalLinks, PageCheckResult pageResult)
        {
            var nonSslLinks = inPostExternalLinks.Where(ipel => ipel.Url.StartsWith("http://"));
            pageResult.AddIssues(nonSslLinks.Select(nssl => $"Non-ssl link {nssl.Url} with anchor text {nssl.AnchorText}."));

            var linkCheckingTasks = new List<Task<string>>(inPostExternalLinks.Select(ipl => GetIssuesWithLink(ipl)));

            await Task.WhenAll(linkCheckingTasks);

            var badResponseCodeLinkProblems = linkCheckingTasks.Select(lct => lct.Result).Where(lcr => !string.IsNullOrEmpty(lcr));
            pageResult.AddIssues(badResponseCodeLinkProblems);
        }

        private void CheckForProblematicWords(Page page, PageCheckResult pageResult)
        {
            var problemWords = GetProblematicWords(page).ToList();
            pageResult.AddIssues(problemWords.Select(pw => $"Contains term \"{pw}\""));
        }

        private void CheckForOutdated(Page page, PageCheckResult pageResult)
        {
            if (ContainsNonCurrentYear(page.Title) || page.H1s.Any(h1 => ContainsNonCurrentYear(h1)))
                pageResult.AddIssue("Is possibly outdated");
        }

        private async Task<string> GetIssuesWithLink(Link linkToCheck)
        {
            var readableLinkDescription = $"link {linkToCheck.Url} with anchor text {linkToCheck.AnchorText?.StripLineBreaks()}";

            try
            {
                var response = await _client.GetHttpResponseFromGetRequestAsync(linkToCheck.Url);
                

                switch (response)
                {
                    case HttpStatusCode.NotFound: return $"404 for {readableLinkDescription}.";
                    case HttpStatusCode.MovedPermanently: return $"Permanent redirect for {readableLinkDescription}.";
                    default: return string.Empty;
                }
            }
            catch
            {
                return $"Link for {readableLinkDescription} generated an error.";
            }
        }

        public IEnumerable<string> GetLinksFrom(string rawPageHtml)
        {
            return rawPageHtml.AsHtml().SelectAttributeValuesForNode("href", "a").Where(link => IsActualLink(link));
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
