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
    public class When_Adding_Cards_To_Trello_WhitepaperSynchronizer_Should
    {
        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private Whitepaper Whitepaper = new Whitepaper()
        {
            Title = "A Whitepaper",
            TargetOutlineDate = new DateTime(8, 1, 1),
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
            Card.Arrange(c => c.ListName).Returns(CalendarService.PlannedPostsListName);

            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { Card });

            Target = new WhitepaperSynchronizer(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Card_Title_To_Whitepaper_Title()
        {
            Target.CreateCardForEntity(Whitepaper);

            Board.Assert(b => b.AddPlannedPostCard(Whitepaper.Title, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.IsAny<string[]>()));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Card_DueDate_To_The_Outline_Date()
        {
            Target.CreateCardForEntity(Whitepaper);
            
            var outlineDate = Whitepaper.TargetOutlineDate.Value;
            var nearMidnight = new DateTime(outlineDate.Year, outlineDate.Month, outlineDate.Day, 23, 59, 59);
            TimeSpan easternTimeUtcOffset = new TimeSpan(-5, 0, 0);
            var nearMidnightOffset = new DateTimeOffset(nearMidnight, easternTimeUtcOffset);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Whitepaper.TargetOutlineDate.SafeToMidnightEastern(), Arg.AnyString, Arg.IsAny<string[]>()));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Users_To_Author_And_Editor_Trello_Ids()
        {
            Target.CreateCardForEntity(Whitepaper);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.Matches<string[]>(sa => sa.Contains(Whitepaper.Author.TrelloId) && sa.Contains(Whitepaper.Editor.TrelloId))));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Trello_Card_CompanyName_To_Whitepaper_Blog_Name()
        {
            Target.CreateCardForEntity(Whitepaper);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Whitepaper.Blog.CompanyName, Arg.IsAny<string[]>()));
        }
}
}
