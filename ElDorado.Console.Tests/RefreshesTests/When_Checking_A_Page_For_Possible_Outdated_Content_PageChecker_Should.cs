using ElDorado.Refreshes;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests.RefreshesTests
{
    [TestClass]
    public class When_Checking_A_Page_For_Possible_Outdated_Content_PageChecker_Should
    {
        private PageChecker Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new PageChecker();

            Target.Now = new DateTime(2020, 1, 1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Page_Is_Empty()
        {
            var rawPageHtml = "<html></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Page_Title_Is_From_A_Previous_Year()
        {
            var rawPageHtml = "<html><title>Best Stuff in 2019</title></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Page_Title_Is_From_Several_Year_Ago()
        {
            var rawPageHtml = "<html><title>Best Stuff in 2016</title></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_If_The_Title_In_The_Year_Is_Pre_2000()
        {
            var rawPageHtml = "<html><title>Best Stuff in 1998</title></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_If_The_Title_Year_Is_This_Year()
        {
            var rawPageHtml = "<html><title>Best Stuff in 2020</title></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_If_Title_Year_Is_In_Future()
        {
            var rawPageHtml = "<html><title>Best Stuff in 2070</title></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_If_An_H1_Has_Last_Year()
        {
            var rawPageHtml = "<html><h1>Best Stuff in 2019</h1></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Returns_False_If_H1_Has_This_Year()
        {
            var rawPageHtml = "<html><h1>Best Stuff in 2020</h1></html>";

            Target.IsPossiblyOutdated(rawPageHtml).ShouldBeFalse();
        }
    }
}
