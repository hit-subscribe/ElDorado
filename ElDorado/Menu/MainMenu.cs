using ElDorado.Domain;
using ElDorado.Metrics;
using ElDorado.WritingCalendar;
using Gold.ConsoleMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Menu
{
    [MenuClass("Main Menu")]
    public class MainMenu
    {
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

        [MenuMethod("Update Trello based on Post Plan")]
        public static void UpdateTrelloWritingCalendar()
        {
            var trelloService = new TrelloWritingCalendarService();
            trelloService.Initialize(new CredentialStore(File.ReadAllText(@"CredFiles\trello.cred")));

            var synchornizer = new BlogPostSynchronizer(trelloService, new PlanningSpreadsheetService());
            synchornizer.UpdatePlannedInTrello();
        }

        [MenuMethod("Add posts from spreadsheet to database")]
        public static void AddPostsToDatabase()
        {
            var spreadsheetService = new PlanningSpreadsheetService();
            var posts = spreadsheetService.GetPlannedPosts("Archive!A2:T").ToList();

            using (var context = new BlogContext())
            {
                foreach(var post in posts)
                {
                    post.Blog = context.Blogs.First(b => b.CompanyName == post.Blog.CompanyName);
                }
                context.BlogPosts.AddRange(posts);
                context.SaveChanges();

                spreadsheetService.UpdatePostIds(context.BlogPosts.ToList(), "Archive!A2:T");
            }

        }
    }
}
