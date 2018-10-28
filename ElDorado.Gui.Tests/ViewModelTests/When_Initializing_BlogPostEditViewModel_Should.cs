using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock.EntityFramework;

namespace ElDorado.Gui.Tests.ViewModelTests
{
    [TestClass]
    public class When_Initializing_BlogPostEditViewModel_Should
    {

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_An_Empty_Authors_List_When_No_Context_Is_Specified()
        {
            var target = new BlogPostEditViewModel();

            target.Authors.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_An_Empty_Authors_List_When_Context_Has_No_Authors()
        {
            var target = new BlogPostEditViewModel(new BlogPost(), EntityFrameworkMock.Create<BlogContext>());

            target.Authors.ShouldBeEmpty();
        }

}
}
