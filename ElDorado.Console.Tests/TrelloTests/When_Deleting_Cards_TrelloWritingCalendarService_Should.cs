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
    public class When_Deleting_Cards_TrelloWritingCalendarService_Should
    {
        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private static readonly string CardId = "ahsdf9";

        private TrelloWritingCalendarService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Card.Arrange(c => c.Id).Returns(CardId);

            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { Card });

            Target = new TrelloWritingCalendarService(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Invokes_Delete_On_A_Card_With_Matching_Id()
        {
            Target.DeleteCard(CardId);

            Card.Assert(c => c.Delete(), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Delete_A_NonMatching_Card()
        {
            Target.DeleteCard("asdf");

            Card.Assert(c => c.Delete(), Occurs.Never());
        }
}
}
