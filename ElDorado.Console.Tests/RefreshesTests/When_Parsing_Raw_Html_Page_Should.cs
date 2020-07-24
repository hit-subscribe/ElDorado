using ElDorado.Console.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock.AutoMock.Ninject.Planning.Targets;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Parsing_Raw_Html_Page_Should
    {
        private const string Url = "http://daedtech.com";
        private const string AnchorText = "blog post";
        private const string PageUrl = "https://raygun.com/blog/best-real-user-monitoring-tools/";
        private const string PageTitle = "The 7 best User Experience Monitoring tools for 2020 &middot; Raygun Blog";
        private static readonly string PageHtml = $"<html><title>{PageTitle}</title><body>A <a href=\"{Url}\">{AnchorText}</a>.</body></html>";


        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Replace_Html_Encoded_Middle_Dot_With_Ascii_Character()
        {
            var target = new Page(PageHtml, PageUrl);

            target.Title.ShouldBe("The 7 best User Experience Monitoring tools for 2020 · Raygun Blog");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Link_In_Body_To_InPostLinks_Collection()
        {
            var target = new Page(PageHtml, PageUrl);

            target.InPostExternalLinks.First().Url.ShouldBe(Url);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Ignore_Links_Not_In_Page_Body()
        {
            var pageHtml = $"<html><a href=\"{Url}\">{AnchorText}</a><title>Asdf</title><body>A post</body></html>";

            var target = new Page(pageHtml, PageUrl);

            target.InPostExternalLinks.Count().ShouldBe(0);
        }
}
}
