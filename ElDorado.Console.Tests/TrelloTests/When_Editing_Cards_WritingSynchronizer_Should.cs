using ElDorado.Domain;
using ElDorado.Trello;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.TrelloTests
{
    [TestClass]
    public class When_Editing_Cards_WritingSynchronizer_Should
    {
        private const string CardTrelloId = "asdfh";

        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private Whitepaper Whitepaper = new Whitepaper()
        {
            Title = "A Whitepaper",
            TargetOutlineDate = new DateTime(8, 1, 1),
            TrelloId = CardTrelloId,
            Author = new Author()
            {
                TrelloId = "Erik.Dietrich"
            },
            Editor = new Editor()
            {
                TrelloId = "Amanda.Muledy"
            },
            Blog = new Blog()
            {
                CompanyName = "Some Company"
            }
        };

        private WhitepaperSynchronizer Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Card.Arrange(c => c.Id).Returns(CardTrelloId);
            Card.Arrange(c => c.ListName).Returns(CalendarService.PlannedPostsListName);

            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { Card });

            Target = new WhitepaperSynchronizer(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Update_The_Title_When_No_Card_Matches()
        {
            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { });

            Target.UpdateCardForEntity(Whitepaper);

            Mock.AssertSet(() => Card.Name = Whitepaper.Title, Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Card_To_Whitepaper_Title()
        {
            Target.UpdateCardForEntity(Whitepaper);

            Mock.AssertSet(() => Card.Name = Whitepaper.Title, Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Card_DueDate()
        {
            var outlineDate = Whitepaper.TargetOutlineDate.Value;
            var nearMidnight = new DateTime(outlineDate.Year, outlineDate.Month, outlineDate.Day, 23, 59, 59);
            TimeSpan easternTimeUtcOffset = new TimeSpan(-5, 0, 0);
            var nearMidnightOffset = new DateTimeOffset(nearMidnight, easternTimeUtcOffset);

            Target.UpdateCardForEntity(Whitepaper);

            Mock.AssertSet(() => Card.DueDate = Whitepaper.TargetOutlineDate.SafeToMidnightEastern(), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Set_Trello_Card_DueDate_When_Card_Is_Not_In_Planned()
        {
            Card.Arrange(c => c.ListName).Returns("asdf");

            Target.UpdateCardForEntity(Whitepaper);

            Mock.AssertSet(() => Card.DueDate = Arg.IsAny<DateTime>(), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Card_Labels_To_CompanyName()
        {
            Target.UpdateCardForEntity(Whitepaper);

            Card.Assert(c => c.UpdateLabels(Board.GetLabelsForCompany(Whitepaper.Blog.CompanyName)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Set_Trello_Card_Labels_When_In_Another_Column()
        {
            Card.Arrange(c => c.ListName).Returns("asdf");
            
            Target.UpdateCardForEntity(Whitepaper);

            Card.Assert(c => c.UpdateLabels(Board.GetLabelsForCompany(Whitepaper.Blog.CompanyName)), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Card_Members_To_Author_And_Editor()
        {
            Target.UpdateCardForEntity(Whitepaper);

            Card.Assert(c => c.UpdateMembers(Board.GetMembersWithUserNames(Whitepaper.Author.TrelloId, Whitepaper.Editor.TrelloId)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Set_Trello_Card_Members_When_Not_In_Planned_Posts()
        {
            Card.Arrange(c => c.ListName).Returns("asdf");

            Target.UpdateCardForEntity(Whitepaper);

            Card.Assert(c => c.UpdateMembers(Board.GetMembersWithUserNames(Whitepaper.Author.TrelloId, Whitepaper.Editor.TrelloId)), Occurs.Never());
        }
}
}
