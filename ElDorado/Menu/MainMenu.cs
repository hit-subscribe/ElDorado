using ElDorado.Domain;
using ElDorado.Metrics;
using ElDorado.Refreshes;
using ElDorado.Repository;
using ElDorado.Scraping;
using ElDorado.Trello;
using ElDorado.Wordpress;
using ElDorado.WritingCalendar;
using Gold.ConsoleMenu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ElDorado.Menu
{
    [MenuClass("Main Menu")]
    public class MainMenu
    {

        [MenuMethod("AnalyzeAPage")]
        public static void AnalyzeAPage()
        {
            Console.WriteLine("Url, please.");
            var url = Console.ReadLine();

            var client = new SimpleWebClient();
            var pageChecker = new PageChecker();

            var rawHtml = client.GetRawResultOfBasicGetRequest(url);

            pageChecker.GetLinksFrom(rawHtml).ToList().ForEach(l => Console.WriteLine(l));

        }

        [MenuMethod("Look for refresh candidates")]
        public static void LookForOutdatedPosts()
        {
            Console.WriteLine("Give me the sitemap, please.");
            var sitemapFilePath = Console.ReadLine();

            var client = new SimpleWebClient();
            var pageChecker = new PageChecker() { ProblemTerms = new List<string>() { "master", "slave", "lame", "retard", "crazy", "derp", "ocd", "gyp", "jip", "ghetto", "hysterical", "dumb", "cripple" } };
            var sitemap = new Sitemap(sitemapFilePath);

            var blogPostUrls = sitemap.SiteUrls;

            var problems = new List<string>();

            foreach(var siteUrl in blogPostUrls)
            {
                var rawHtml = client.GetRawResultOfBasicGetRequest(siteUrl.Url);

                if(pageChecker.IsPossiblyOutdated(rawHtml))
                    problems.Add($"{siteUrl.Url} was last modified {siteUrl.LastUpdated} and might be outdated.");

                var problemWords = pageChecker.GetProblematicWords(rawHtml);
                if(problemWords.Any())
                    problems.Add($"{siteUrl.Url} contains problematic langauge: {String.Join(" ", problemWords)}.");
            }

            File.WriteAllText(@"C:\users\erik\desktop\problems.txt", String.Join("\n", problems));

        }

        [MenuMethod("Record blog metrics")]
        public static void RecordMetrics()
        {
            var webClient = new SimpleWebClient();
            var feedlyInquisitor = new FeedlyInquisitor(webClient);
            var alexaInquisitor = new AlexaDataInquisitor(webClient);
            var mozInquisitor = new MozInquisitor(new CredentialStore(File.ReadAllText(@"CredFiles\moz.cred")), webClient) { Timeout = 10 };

            var metricsRecorder = new BlogMetricsRecorder(feedlyInquisitor, alexaInquisitor, mozInquisitor);

            using (var context = new BlogContext())
            {
                var blogs = context.Blogs.ToList();
                var blogMetrics = metricsRecorder.GenerateMetrics(blogs);
                context.BlogMetrics.AddRange(blogMetrics);
                context.SaveChanges();
            }
        }

        [MenuMethod("Scrape Search")]
        public static void ScrapeSearch()
        {
            Console.WriteLine("Enter search term");
            var searchTerm = Console.ReadLine();

            var webClient = new SimpleWebClient();
            var retriever = new SearchResultRetriever(webClient, new CredentialStore(File.ReadAllText(@"CredFiles\cse.cred")));

            const int serpPagesToCrawl = 5;
            var results = retriever.SearchFor(searchTerm, serpPagesToCrawl).ToList();

            var csvRows = results.Select(res => $"{res.DisplayLink},{res.Link}").ToList();
            csvRows.Insert(0, "Base Site,Resut Link");

            File.WriteAllLines("results.csv", csvRows);
        }

    }
}
