using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    class Program
    {
        private const string RootDirectory = @"C:\Users\Erik\Dropbox\Hit Subscribe\Data";

        private static readonly FeedlyInquisitor _feedlyInquisitor = new FeedlyInquisitor(new SimpleWebClient());
        private static readonly AlexaDataInquisitor _alexaInquisitor = new AlexaDataInquisitor(new SimpleWebClient());

        static void Main(string[] args)
        {
            var inputFileLines = File.ReadAllLines($@"{RootDirectory}\blogs.csv");

            string outputFile = $@"{RootDirectory}\stats.csv";

            File.AppendAllLines(outputFile, inputFileLines.Select(fl => BuildLineFromBlogStatsRecord(fl)));

        }

        private static string BuildLineFromBlogStatsRecord(string inputFileLine)
        {
            string baseSite = inputFileLine.Split(',')[1];
            string feedlyUrl = inputFileLine.Split(',')[2];
            var statsRecord = new BlogStatsRecord(inputFileLine)
            {
                Timestamp = DateTime.Now,
                SubscriberCount = _feedlyInquisitor.GetSubscriberCount(feedlyUrl),
                AlexaRank = _alexaInquisitor.GetGlobalRank(baseSite)
            };

            return statsRecord.ToCsv();
        }
    }
}
