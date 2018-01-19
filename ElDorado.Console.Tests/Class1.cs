using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests
{
    [TestClass]
    public class When_Searching_For_Subscriber_Count_FeedlyInquisitor_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_By_Default()
        {
            var inquisitor = new FeedlyInquisitor();

            //var count = inquisitor.GetSubscriberCount("http://cloud.feedly.com/v3/feeds/feed%2Fhttp%3A%2F%2Fwww.daedtech.com%2Ffeed");
            var count = inquisitor.GetSubscriberCount(string.Empty);

            count.ShouldBe(0);
        }
}
    public class FeedlyInquisitor
    {

        public int GetSubscriberCount(string str)
        {
            return 0;
        }
    }
}
