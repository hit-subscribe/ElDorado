using ElDorado.Refreshes;
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
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Replace_Html_Encoded_Middle_Dot_With_Ascii_Character()
        {
            var pageHtml = "<html><title>The 7 best User Experience Monitoring tools for 2020 &middot; Raygun Blog</title><body>A blog post.</body></html>";
            var target = new Page(pageHtml, "https://raygun.com/blog/best-real-user-monitoring-tools/");

            target.Title.ShouldBe("The 7 best User Experience Monitoring tools for 2020 · Raygun Blog");
        }
    }
}
