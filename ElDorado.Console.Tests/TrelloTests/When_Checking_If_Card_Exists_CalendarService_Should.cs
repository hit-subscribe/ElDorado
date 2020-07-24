using ElDorado.Console.Trello;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.TrelloTests
{
    [TestClass]
    public class When_Checking_If_Card_Exists_CalendarService_Should
    {
        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private static readonly string CardTitle = "A Blog Post";

        private CalendarService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Card.Arrange(c => c.Name).Returns(CardTitle);

            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { Card });

            Target = new WritingCalendarService(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_The_Board_Has_No_Cards()
        {
            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>());

            Target.DoesCardExistWithTitle(CardTitle).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Board_Has_A_Card_That_Matches_The_Title()
        {
            Target.DoesCardExistWithTitle(CardTitle).ShouldBeTrue();
        }
}
}
