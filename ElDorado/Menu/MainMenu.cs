using ElDorado.Console.Metrics;
using ElDorado.Console.Refreshes;
using ElDorado.Console.Trello;
using ElDorado.Domain;
using ElDorado.Refreshes;
using Gold.ConsoleMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using static System.Console;

namespace ElDorado.Console.Menu
{
    [MenuClass("Main Menu")]
    public class MainMenu
    {

        [MenuMethod("AnalyzeAPage")]
        public static void AnalyzeAPage()
        {
            
            WriteLine("Url, please.");
            var url = System.Console.ReadLine();

            var client = new SimpleWebClient();
            var pageChecker = new PageChecker();

            var rawHtml = client.GetRawResultOfBasicGetRequest(url);

            pageChecker.GetLinksFrom(rawHtml).ToList().ForEach(l => WriteLine(l));

        }

        [MenuMethod("Look for refresh candidates")]
        public static void LookForOutdatedPosts()
        {
            WriteLine("Give me the sitemap, please.");
            var sitemapFilePath = ReadLine();

            try
            {
                var sitemap = new Sitemap(sitemapFilePath);

                var client = new SimpleWebClient();
                var pageChecker = new PageChecker(client)
                {
                    ProblemTerms = new List<string>()
                { "master", "slave", "blacklist", "whitelist", "lame", "retard", "crazy", "derp", "ocd", "gyp", "jip", "ghetto", "hysterical", "dumb", "cripple" }
                };

                var auditResult = pageChecker.AuditSiteFromSiteMap(sitemap).Result;

                File.WriteAllText(@"C:\users\erik\desktop\problems.csv", auditResult.ProblemsToCsv(), Encoding.UTF8);
            }
            catch
            {
                WriteLine("Oops, try again.");
            }

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
            WriteLine("Enter search term");
            var searchTerm = ReadLine();

            var webClient = new SimpleWebClient();
            var retriever = new SearchResultRetriever(webClient, new CredentialStore(File.ReadAllText(@"CredFiles\cse.cred")));

            const int serpPagesToCrawl = 5;
            var results = retriever.SearchFor(searchTerm, serpPagesToCrawl).ToList();

            var csvRows = results.Select(res => $"{res.DisplayLink},{res.Link}").ToList();
            csvRows.Insert(0, "Base Site,Resut Link");

            File.WriteAllLines(@"C:\users\erik\desktop\results.csv", csvRows);
        }

    }
}
