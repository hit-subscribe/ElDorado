using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telerik.JustMock;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Providing_A_Collection_Of_SiteUrls_Sitemap_Urls_Should
    {
        //I took these from MMAP's actual site map
        private static readonly string EmptySiteMap = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><?xml-stylesheet type=\"text/xsl\" href=\"//makemeaprogrammer.com/wp-content/plugins/wordpress-seo/css/main-sitemap.xsl\"?><urlset xmlns:xsi =\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"></urlset>";
        private static readonly string SingleUrlSiteMap = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><?xml-stylesheet type=\"text/xsl\" href=\"//makemeaprogrammer.com/wp-content/plugins/wordpress-seo/css/main-sitemap.xsl\"?><urlset xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">	<url>		<loc>https://makemeaprogrammer.com/</loc>		<lastmod>2020-01-18T20:01:28+00:00</lastmod>	</url></urlset>";

        private XDocument Document { get; set; }

        private Sitemap Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Document = new XDocument();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_Collection_When_No_Urls()
        {
            Document = XDocument.Parse(EmptySiteMap);

            Target = new Sitemap(Document);

            Target.SiteUrls.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Single_Site_Url_For_One_Entry()
        {
            Document = XDocument.Parse(SingleUrlSiteMap);

            Target = new Sitemap(Document);

            Target.SiteUrls.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_SiteUrl_With_MMAP_Root_Address_For_One_Entry()
        {
            Document = XDocument.Parse(SingleUrlSiteMap);

            Target = new Sitemap(Document);

            Target.SiteUrls.First().Url.ShouldBe("https://makemeaprogrammer.com/");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Url_Last_Modified_Of_January_18_2020()
        {
            Document = XDocument.Parse(SingleUrlSiteMap);

            Target = new Sitemap(Document);

            Target.SiteUrls.First().LastUpdated.Value.DayOfYear.ShouldBe(18);
        }
}
}
