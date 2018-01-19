using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests
{
    [TestClass]
    public class When_Searching_For_Subscriber_Count_FeedlyInquisitor_Should
    {
        private SimpleWebClient Client = Mock.Create<SimpleWebClient>();

        private FeedlyInquisitor Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(c => c.GetRawText("http://cloud.feedly.com/v3/feeds/feed%2Fhttp%3A%2F%2Fwww.daedtech.com%2Ffeed")).Returns("{\"iconUrl\":\"https://storage.googleapis.com/site-assets/V7PyrZNy_9OWWjQxM3FiFuemnkUD-4PXYw33r-WuSPM_icon-15d5ed346c5\",\"visualUrl\":\"https://storage.googleapis.com/site-assets/V7PyrZNy_9OWWjQxM3FiFuemnkUD-4PXYw33r-WuSPM_visual-15d5ed346c5\",\"coverUrl\":\"https://storage.googleapis.com/site-assets/V7PyrZNy_9OWWjQxM3FiFuemnkUD-4PXYw33r-WuSPM_cover-15d5ed346c5\",\"id\":\"feed/http://www.daedtech.com/feed\",\"feedId\":\"feed/http://www.daedtech.com/feed\",\"title\":\"DaedTech\",\"subscribers\":1884,\"updated\":1516330626191,\"velocity\":2.3,\"website\":\"https://www.daedtech.com\",\"topics\":[\"programming\",\"tech\",\"development\",\"dev\"],\"partial\":false,\"language\":\"en\",\"contentType\":\"longform\",\"description\":\"DaedTech LLC: programming, architecture, IT management consulting, and blogging.\",\"coverColor\":\"131516\"}");
            Target = new FeedlyInquisitor(Client);   
        }


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

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_For_Nonsense()
        {
            Target.GetSubscriberCount("asdfASfd").ShouldBe(0);
        }
}

}
