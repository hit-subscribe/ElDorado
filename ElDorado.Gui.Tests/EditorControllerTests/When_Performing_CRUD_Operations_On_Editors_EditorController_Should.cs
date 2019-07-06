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

namespace ElDorado.Gui.Tests.EditorControllerTests
{
    [TestClass]
    public class When_Performing_CRUD_Operations_On_Editors_EditorController_Should
    {
        private const int EditorId = 1211;
        private const string EditorName = "Amanda";

        private Editor Editor { get; } = new Editor() { Id = EditorId, FirstName = EditorName, LastName = "Muledy" };

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private EditorsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Editors.Add(Editor);

            Target = new EditorsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_All_Editors_In_Index()
        {
            var originalEditorCount = Context.Editors.Count();

            var additionalEditors = new List<Editor> { new Editor(), new Editor() };
            Context.Editors.AddRange(additionalEditors);

            var editors = Target.Index().GetResult<IEnumerable<Editor>>();

            editors.Count().ShouldBe(originalEditorCount + additionalEditors.Count());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Sort_Results_By_Editor_FirstName()
        {
            const string deliberatelyAlphabeticalFirstName = "AAAAAAA";

            Context.Editors.Add(new Editor() { Id = 81111, FirstName = deliberatelyAlphabeticalFirstName });

            var editors = Target.Index().GetResult<IEnumerable<Editor>>();

            editors.First().FirstName.ShouldBe(deliberatelyAlphabeticalFirstName);
        }
    }
}
