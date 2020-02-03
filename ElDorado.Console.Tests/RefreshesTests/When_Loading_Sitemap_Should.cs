using ElDorado.Refreshes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

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
    }
}
