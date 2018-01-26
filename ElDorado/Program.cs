using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    class Program
    {
        static void Main(string[] args)
        {
            var inquistor = new FeedlyInquisitor(new SimpleWebClient());
            //var count = inquistor.GetSubscriberCount("http://cloud.feedly.com/v3/feeds/feed%2Fhttp%3A%2F%2Ffeeds.feedburner.com%2FGrabBagOfT");
            var count = inquistor.GetSubscriberCount("https://cloud.feedly.com/v3/feeds/feed%2Fhttp%3A%2F%2Fwww.daedtech.com%2Ffeed");

            Console.WriteLine(count);

            Console.ReadLine();
     
        }
    }
}
