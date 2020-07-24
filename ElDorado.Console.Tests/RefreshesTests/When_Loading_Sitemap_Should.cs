using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Xml.Linq;
using Telerik.JustMock;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Loading_Sitemap_Should
    {
        private static readonly string Filepath = "Some filepath";

        private XDocument Document { get; set; } = Mock.Create<XDocument>();

        private Sitemap Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new Sitemap(Filepath, Document);
        }


        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_Exception_On_Null_Path()
        {
            Should.Throw<ArgumentNullException>(() => new Sitemap((string)null));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_SiteUrl_LastModified_To_Null_When_Missing()
        {
            var document = XDocument.Parse("<?xml version=\"1.0\" encoding=\"UTF - 8\"?><urlset xmlns = \"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"><url><loc>https://aspetraining.com/</loc><priority>1</priority></url></urlset>");

            Target = new Sitemap(document);

            Target.SiteUrls.First().LastUpdated.ShouldBeNull();
        }
    }
}
