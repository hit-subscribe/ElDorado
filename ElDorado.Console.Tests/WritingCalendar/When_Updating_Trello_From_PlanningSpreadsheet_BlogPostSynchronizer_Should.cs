using ElDorado.Domain;
using ElDorado.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.WritingCalendar
{
    [TestClass]
    public class When_Updating_Trello_From_PlanningSpreadsheet_BlogPostSynchronizer_Should
    {
        private TrelloWritingCalendarService TrelloWritingCalendarService { get; } = Mock.Create<TrelloWritingCalendarService>();
        private PlanningSpreadsheetService PlanningSpreadsheetService { get; } = Mock.Create<PlanningSpreadsheetService>();

        private static readonly BlogPost UnapprovedPostFromSpreadsheet = new BlogPost() { Title = "An Unapproved Blog Post", IsApproved = false };
        private static readonly BlogPost AprpovedPostFromSpreadsheet = new BlogPost() { Title = "A Blog Post", IsApproved = true };
        private static readonly BlogPost PostThatAlreadyExistsInTrello = new BlogPost() { Title = "A Blog Post Already In Trello", IsApproved = true };

        private BlogPostSynchronizer Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            PlanningSpreadsheetService.Arrange(pss => pss.GetPosts(Arg.AnyString)).Returns(new List<BlogPost>() { UnapprovedPostFromSpreadsheet, AprpovedPostFromSpreadsheet, PostThatAlreadyExistsInTrello });
            TrelloWritingCalendarService.Arrange(twcs => twcs.DoesCardExist(PostThatAlreadyExistsInTrello.Title)).Returns(true);

            Target = new BlogPostSynchronizer(TrelloWritingCalendarService, PlanningSpreadsheetService);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_The_Blog_Posts_Title()
        {
            Target.UpdatePlannedInTrello();

            TrelloWritingCalendarService.Assert(s => s.AddCard(AprpovedPostFromSpreadsheet), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Add_A_Post_Already_In_Trello_Planned_Posts()
        {
            Target.UpdatePlannedInTrello();

            TrelloWritingCalendarService.Assert(s => s.AddCard(PostThatAlreadyExistsInTrello), Occurs.Never());
        }
    }
}
