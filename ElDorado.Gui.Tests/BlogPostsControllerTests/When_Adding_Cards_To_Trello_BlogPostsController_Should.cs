using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using ElDorado.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.BlogPostsControllerTests
{
    [TestClass]
    public class When_Adding_Cards_To_Trello_BlogPostsController_Should
    {
        private const string PostTitle = "Facepalming 101";
        private const int PostId = 123;

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private TrelloWritingCalendarService CalendarService { get; } = Mock.Create<TrelloWritingCalendarService>();

        private BlogPost Post { get; set; } = new BlogPost() { Id = PostId, Title = PostTitle };

        private BlogPostsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogPostsController(Context, CalendarService);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Call_TrelloService_Add_Card_Method()
        {
            bool wasCalled = false;
            CalendarService.Arrange(cs => cs.AddCard(Arg.IsAny<BlogPost>())).DoInstead(() => wasCalled = true);

            Target.Edit(new BlogPostEditViewModel());

            wasCalled.ShouldBeTrue();
        }
}
}
