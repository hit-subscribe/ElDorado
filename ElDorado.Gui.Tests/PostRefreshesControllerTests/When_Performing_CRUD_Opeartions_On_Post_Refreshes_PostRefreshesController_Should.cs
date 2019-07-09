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

namespace ElDorado.Gui.Tests.PostRefreshesControllerTests
{
    [TestClass]
    public class When_Performing_CRUD_Opeartions_On_Post_Refreshes_PostRefreshesController_Should
    {
        private const int BlogPostId = 1;
        
        private BlogPost Post = new BlogPost() { Id = BlogPostId };
        private PostRefresh Refresh = new PostRefresh() { BlogPostId = BlogPostId };
        
        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private PostRefreshesController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.PostRefreshes.Add(Refresh);

            Target = new PostRefreshesController(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_All_Refreshes_In_Index()
        {
            var originalRefreshCount = Context.PostRefreshes.Count();

            var additionalRefreshes = new List<PostRefresh>() { new PostRefresh() };
            Context.PostRefreshes.AddRange(additionalRefreshes);

            var refreshes = Target.Index().GetResult<IEnumerable<PostRefresh>>();

            refreshes.Count().ShouldBe(originalRefreshCount + additionalRefreshes.Count());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Sort_Results_By_DraftDate()
        {
            const int earliestRefreshId = 12;
            Refresh.DraftDate = new DateTime(2019, 4, 1);
            Context.PostRefreshes.Add(new PostRefresh() { DraftDate = new DateTime(2019, 1, 1), Id = earliestRefreshId });

            var refreshes = Target.Index().GetResult<IEnumerable<PostRefresh>>();

            refreshes.First().Id.ShouldBe(earliestRefreshId);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_No_Results_When_BlogPostId_Is_Specified_But_Matches_Nothing()
        {
            var refreshes = Target.Index(BlogPostId + 1).GetResult<IEnumerable<PostRefresh>>();

            refreshes.ShouldBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_Result_When_BlogPostId_Matches()
        {
            var refreshes = Target.Index(BlogPostId).GetResult<IEnumerable<PostRefresh>>();

            refreshes.ShouldNotBeEmpty();
        }
}
    public class PostRefreshesController : CrudController<PostRefresh>
    {
        public PostRefreshesController(BlogContext context) : base(context)
        {
            IndexSortFunction = refreshes => refreshes.OrderBy(r => r.DraftDate);
        }
        public ActionResult Index(int blogPostId)
        {
            var matchingRefreshes = Context.PostRefreshes.Where(pr => pr.BlogPostId == blogPostId);
            return View(IndexSortFunction(matchingRefreshes));
        }
    }
}
