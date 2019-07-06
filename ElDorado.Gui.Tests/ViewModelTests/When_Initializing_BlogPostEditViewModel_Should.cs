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
        private Editor Editor => Context.Editors.First();

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Editors.Add(new Editor() { Id = 123, FirstName = "Amanda", LastName = "Muledy", IsActive = true, IsInOurSystems = true });   
        }

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

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_An_Empty_Editors_List_When_No_Context_Is_Specified()
        {
            var target = new BlogPostEditViewModel();

            target.Editors.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_An_Editor_When_One_Exists_In_Context()
        {
            var target = new BlogPostEditViewModel(new BlogPost(), Context);

            target.Editors.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_Editors_Sorted_By_FirstName()
        {
            const string nameThatShouldBeFirst = "AAAAA";
            Context.Editors.Add(new Editor() { Id = 1231234, FirstName = nameThatShouldBeFirst, LastName = "Whatever", IsInOurSystems = true, IsActive = true });

            var target = new BlogPostEditViewModel(new BlogPost(), Context);

            target.Editors.First().Text.ShouldContain(nameThatShouldBeFirst);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Filter_Out_Editors_Not_In_Our_Systems()
        {
            Editor.IsInOurSystems = false;
            
            var target = new BlogPostEditViewModel(new BlogPost(), Context);

            target.Editors.ShouldBeEmpty();
        }

}   
}
