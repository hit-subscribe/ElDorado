using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Getting_Multiple_PageResults_Async_AuditService_Should
    {
        private SimpleWebClient Client { get; } = Mock.Create<SimpleWebClient>();

        private AuditService Target { get; set; }

        private void ArrangePage(string html)
        {
            Client.Arrange(c => c.GetRawResultOfBasicGetRequestAsync(Arg.AnyString)).Returns(Task.FromResult(html));
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new AuditService(Client);

            Target.Now = new DateTime(2020, 1, 1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Two_Results_For_Two_Pages()
        {
            ArrangePage(string.Empty);

            var pageResults = Target.GetPageResultsAsync(new List<string> { "http://firstUrl.com", "http://secondUrl.whatever" });

            pageResults.Result.Count().ShouldBe(2);
        }
}
}
