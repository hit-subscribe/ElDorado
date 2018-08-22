using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.JustMock.EntityFramework;

namespace ElDorado.Gui.Tests.AuthorControllerTests
{
    [TestClass]
    public class When_Retrieving_The_AuthorIndex_AuthorControllerShould
    {
        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private AuthorsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new AuthorsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_All_Authors_In_Context()
        {
            var authorsInContext = new List<Author>() { new Author(), new Author(), new Author() };
            Context.Authors.AddRange(authorsInContext);

            var authors = Target.Index().GetViewResultModel<IEnumerable<Author>>();

            authors.Count().ShouldBe(authorsInContext.Count());
        }
    }

}
