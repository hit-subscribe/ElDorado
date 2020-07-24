using ElDorado.Console.Metrics;
using ElDorado.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests
{
    [TestClass]
    public class When_Recording_BlogMetrics_BlogMetricsRecorder_Should
    {
        private Blog TargetBlog { get; set; } = new Blog() { Id = 123, Url = "http://asdf.com", FeedlyUrl = "blah" };

        private IEnumerable<Blog> TargetBlogs => TargetBlog.AsEnumerable();

        private FeedlyInquisitor FeedlyInquisitor { get; set; } = Mock.Create<FeedlyInquisitor>();

        private AlexaDataInquisitor AlexaInquisitor { get; set; } = Mock.Create<AlexaDataInquisitor>();

        private MozInquisitor MozInquisitor { get; set; } = Mock.Create<MozInquisitor>();

        private BlogMetricsRecorder Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogMetricsRecorder(FeedlyInquisitor, AlexaInquisitor, MozInquisitor);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Fetch_Subscriber_Count_From_FeedlyInquisitor_Using_Blogs_Feedly_Url()
        {
            const int feedlySubscriberCount = 200;
            FeedlyInquisitor.Arrange(fi => fi.GetSubscriberCount(TargetBlog.FeedlyUrl)).Returns(feedlySubscriberCount);

            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.First().FeedlySubscribers.ShouldBe(feedlySubscriberCount);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Fetch_AlexaRank_From_AlexaInquisitor_Using_Blogs_Url()
        {
            const int alexaRank = 123456;
            AlexaInquisitor.Arrange(ai => ai.GetGlobalRank(TargetBlog.Url)).Returns(alexaRank);

            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.First().AlexaRanking.ShouldBe(alexaRank);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Fetch_DomainAuthority_From_MozInquisitor_Using_Blog_Url_HostName()
        {
            const decimal domainAuthority = 22.5M;
            MozInquisitor.Arrange(mi => mi.GetMozStats(TargetBlog.Hostname)).Returns(new MozInquisitor.MozRecord(domainAuthority, 0));

            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.First().DomainAuthority.ShouldBe(domainAuthority);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Fetch_LinkingDomains_From_MozInquisitor_Using_Blog_Url_Hostname()
        {
            const int linkingDomains = 1234;
            MozInquisitor.Arrange(mi => mi.GetMozStats(TargetBlog.Hostname)).Returns(new MozInquisitor.MozRecord(0M, linkingDomains));

            var blogMetrics = Target.GenerateMetrics(TargetBlogs).ToList();

            blogMetrics.First().LinkingRootDomains.ShouldBe(linkingDomains);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_BlogId_To_The_Blogs_Id()
        {
            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.First().BlogId.ShouldBe(TargetBlog.Id);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Recorded_To_Current_Time()
        {
            var timeStamp = new DateTime(2018, 2, 22);
            Target.TimeRecordingStrategy = () => timeStamp;

            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.First().Recorded.ShouldBe(timeStamp);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_FeedlySubscribers_To_Zero_When_FeedlyInquisitor_Throws_Exception()
        {
            FeedlyInquisitor.Arrange(fi => fi.GetSubscriberCount(null)).Throws(new InvalidOperationException());
            TargetBlogs.First().FeedlyUrl = null;

            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.First().FeedlySubscribers.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_AlexaRanking_To_Zero_When_Alexa_Inquisitor_Throws_Exception()
        {
            AlexaInquisitor.Arrange(ai => ai.GetGlobalRank(null)).Throws(new InvalidOperationException());
            TargetBlogs.First().Url = null;

            var blogMetrics = Target.GenerateMetrics(TargetBlogs);

            blogMetrics.Count().ShouldBe(0);
        }
}
}
