using ElDorado.Domain;
using ElDorado.Metrics;
using ElDorado.Repository;
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

        private const string PostPlanningSpreadsheetId = "1BFycG-T2eY3Uh8HWr5c5h-MjYEUJ8eKjqJ8GLxhdh2w";

        private static readonly BlogContext _context = new BlogContext();

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

            var synchornizer = new BlogPostSynchronizer(trelloService, new PlanningSpreadsheetService(new GoogleSheet(PostPlanningSpreadsheetId)));
            synchornizer.UpdatePlannedInTrello();
        }

        [MenuMethod("Add posts from spreadsheet to database")]
        public static void AddPostsToDatabase()
        {
            const string range = "Current!A2:T";

            var repository = new BlogPostRepository(_context);
            var spreadsheetService = new PlanningSpreadsheetService(new GoogleSheet(PostPlanningSpreadsheetId));

            var posts = spreadsheetService.GetPosts(range).ToList();
            repository.Add(posts);

            spreadsheetService.UpdatePostIds(_context.BlogPosts.ToList(), range);
        }

        [MenuMethod("See unconfirmed posts")]
        public static void SeeUnconfirmedPosts()
        {
            var clientSpreadsheetService = new ClientSpreadsheetService(new GoogleSheet("1701JX0uLF65Z-RzCuunn9VY0_2JkDGWEJSD6fS3z2yE"));

            var databasePosts = _context.BlogPosts.ToList();
            var spreadsheetPosts = clientSpreadsheetService.GetPosts().ToList();

            var postsToShow = spreadsheetPosts.Where(sp => !databasePosts.Any(p => p.Title == sp.Title)).ToList();

            foreach (var post in postsToShow)
                Console.WriteLine($"{post.Title} --- due {post.TargetPublicationDate}.");
        }
    }
}
