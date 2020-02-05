using ElDorado.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Retrieving_Links_From_A_Page_PageChecker_Should
    {
        private PageChecker Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new PageChecker();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_No_Links_If_Page_Is_Empty()
        {
            Target.GetLinksFrom("<html></html>").ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Url_Of_Link_When_One_Exists()
        {
            const string url = "https://daedtech.com";
            var rawHtml = $"<html><a href=\"{url}\"></a></html>";

            Target.GetLinksFrom(rawHtml).First().ShouldBe(url);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_When_A_Tag_Exists_With_No_Href()
        {
            Target.GetLinksFrom("<html><a/></html>").ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Empty_When_Only_A_Tag_Is_An_Anchor_Tag()
        {
            Target.GetLinksFrom("<html><a href=\"#someanchor\"/></html>").ShouldBeEmpty();
        }
    }
}
