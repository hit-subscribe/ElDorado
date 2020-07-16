using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain.Tests.LinkTests
{
    [TestClass]
    public class When_Initializing_From_HtmlNode_Link_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_AnchorText_To_Empty_When_A_Tag_Has_No_ChildNodes()
        {
            var node = HtmlNode.CreateNode("<a href=\"https://somelink.com\"></a>");

            new Link(node).AnchorText.ShouldBe(string.Empty);
        }
}
}
