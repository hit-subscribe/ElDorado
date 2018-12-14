using ElDorado.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock.EntityFramework;
using ElDorado.Gui.Controllers;
using System.Data.Entity;
using Shouldly;

namespace ElDorado.Gui.Tests.BlogsControllerTests
{
    [TestClass]
    public class When_Retrieving_The_BlogsIndex_BlogsController_Should
    {
        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Alphabetize_The_Results_By_Company_Name()
        {
            Context.Blogs.Add(new Blog() { CompanyName = "B" });
            Context.Blogs.Add(new Blog() { CompanyName = "A" });

            var target = new BlogsController(Context);

            var viewModel = target.Index().GetResult<IEnumerable<Blog>>();

            viewModel.First().CompanyName.ShouldBe("A");
        }
}
}
