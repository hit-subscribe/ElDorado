using ElDorado.Console.Metrics;
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
    public class When_Initializing_BlogStatsRecord_Should
    {
        private const string BlogName = "DaedTech";
        private static readonly string BlogRecord = $"{BlogName},https://www.daedtech.com,somefeedlyurl";

        private BlogStatsRecord Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogStatsRecord(BlogRecord);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Initialize_BlogName_To_First_Token_Of_File_Record()
        {
            Target.BlogName.ShouldBe(BlogName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_A_ToCsv_Result_Containing_Initialized_Timestamp()
        {
            var timestamp = new DateTime(2018, 1, 12);
            Target.Timestamp = timestamp;

            Target.ToCsv().ShouldContain(timestamp.ToString());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_A_ToCsv_Containing_Initialize_SubscriberCount()
        {
            var subscriberCount = 52341;
            Target.SubscriberCount = subscriberCount;

            Target.ToCsv().ShouldContain(subscriberCount.ToString());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_A_ToCsv_Containing_AlexaRank()
        {
            var alexaRank = 123456;
            Target.AlexaRank = alexaRank;

            Target.ToCsv().ShouldContain(alexaRank.ToString());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_A_Csv_Containing_DomainAuthority()
        {
            var domainAuthority = 22.34M;
            Target.DomainAuthority = domainAuthority;

            Target.ToCsv().ShouldContain(domainAuthority.ToString());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_A_Csv_Containing_LinkingDomains()
        {
            var linkingDomains = 1234;
            Target.LinkingDomains = linkingDomains;

            Target.ToCsv().ShouldContain(linkingDomains.ToString());          
        }
}   

}
