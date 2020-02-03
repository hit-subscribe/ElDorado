using ElDorado.Domain;
using ElDorado.Metrics;
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

        private const string PostPlanningSpreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";

        private static readonly BlogContext _context = new BlogContext();

        [MenuMethod("Look for outdated posts")]
        public static void LookForOutdatedPosts()
        {
            Console.WriteLine("Give me the sitemap, please.");
            var sitemapFilePath = Console.ReadLine();

            var client = new SimpleWebClient();

            var sitemapXml = new XmlDocument();
            sitemapXml.Load(sitemapFilePath);

            var blogPostUrls = sitemapXml.GetElementsByTagName("loc").Cast<XmlNode>().Select(node => node.InnerText); 

            foreach(var url in blogPostUrls)
            {
                var rawHtml = client.GetRawResultOfBasicGetRequest(url).AsHtml();
                var title = rawHtml.SelectNodesWithTag("title").First().InnerText;
                var h1s = rawHtml.SelectNodesWithTag("h1").Select(n => n.InnerText);

                if (title.Contains("2019") || h1s.Any(h1 => h1.Contains("2019")))
                    Console.WriteLine(url);
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
