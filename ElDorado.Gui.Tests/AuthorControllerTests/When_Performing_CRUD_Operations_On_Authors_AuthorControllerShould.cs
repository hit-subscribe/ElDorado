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
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.AuthorControllerTests
{
    [TestClass]
    public class When_Performing_CRUD_Operations_On_Authors_AuthorControllerShould
    {
        private const int AuthorId = 12;
        private const string AuthorName = "Erik";

        private Author Author { get; } = new Author() { Id = AuthorId, FirstName = AuthorName };

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private AuthorsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Authors.Add(Author);

            Target = new AuthorsController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_All_Authors_In_Context_From_Index()
        {
            var originalAuthorCount = Context.Authors.Count();

            var authorsInContext = new List<Author>() { new Author(), new Author(), new Author() };
            Context.Authors.AddRange(authorsInContext);

            var authors = Target.Index().GetViewResultModel<IEnumerable<Author>>();

            authors.Count().ShouldBe(authorsInContext.Count() + originalAuthorCount);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Create_Get_Request_With_Empty_Author()
        {
            var author = Target.Create().GetViewResultModel<Author>();

            author.Id.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Create_Postback_By_Adding_To_Context()
        {
            Target.Create(new Author());

            Context.Authors.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Respond_To_Create_Postback_By_Saving_To_Context()
        {
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Create(new Author());

            Context.Assert(ctx => ctx.SaveChanges());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_To_Edit_From_Postback()
        {
            var actionResult = Target.Create(new Author()) as RedirectToRouteResult;

            actionResult.ShouldHaveRouteAction("Edit");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Author_From_Context_On_Edit_Get_Request()
        {
            var result = Target.Edit(AuthorId).GetViewResultModel<Author>();

            result.FirstName.ShouldBe(AuthorName);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Save_To_Model_On_Postback_For_Edit()
        {
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Edit(Author);

            Context.Assert(ctx => ctx.SaveChanges(), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Author_With_Same_Id_On_Postback()
        {
            var result = Target.Edit(Author).GetViewResultModel<Author>();

            result.Id.ShouldBe(AuthorId);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Attach_Post_To_Authors_Collection_On_Postback()
        {
            Context.Arrange(ctx => ctx.Authors.Attach(Arg.IsAny<Author>()));

            Target.Edit(Author);

            Context.Assert(ctx => ctx.Authors.Attach(Author), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Author_As_Modified_On_Postback()
        {
            Context.Arrange(ctx => ctx.SetModified(Arg.IsAny<Author>()));

            Target.Edit(Author);

            Context.Assert(ctx => ctx.SetModified(Author), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_Removal_Of_Author_From_Context_On_Delete()
        {
            Target.Delete(AuthorId);

            Context.Authors.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Result_In_Saved_Context_On_Delete()
        {
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Delete(AuthorId);

            Context.Assert(ctx => ctx.SaveChanges(), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Redirect_Back_To_Index_After_Delete()
        {
            var actionResult = Target.Delete(AuthorId) as RedirectToRouteResult;

            actionResult.ShouldHaveRouteAction("Index");
        }
}
}
