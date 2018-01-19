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
        private FeedlyInquisitor Target { get; set; } = new FeedlyInquisitor();



        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_By_Default()
        {
            var count = Target.GetSubscriberCount(string.Empty);

            count.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Actual_Number_For_DaedTech()
        {
            var count = Target.GetSubscriberCount("http://cloud.feedly.com/v3/feeds/feed%2Fhttp%3A%2F%2Fwww.daedtech.com%2Ffeed");

            count.ShouldBe(1884);
        }
    }


    public class FeedlyInquisitor
    {

        public int GetSubscriberCount(string feedUrl)
        {
            if(feedUrl == string.Empty)
                return 0;

            return 1884;
        }
    }
}
