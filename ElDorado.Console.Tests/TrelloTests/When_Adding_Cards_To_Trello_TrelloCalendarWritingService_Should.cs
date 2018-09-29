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
    public class When_Adding_Cards_To_Trello_TrelloCalendarWritingService_Should
    {
        private IWritingCalendarBoard Board = Mock.Create<IWritingCalendarBoard>();

        private TrelloWritingCalendarService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new TrelloWritingCalendarService(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_Name_Set_To_Post_AuthorTitle()
        {
            const string postTitle = "asdf";
            Target.AddCard(new BlogPost() { Title = postTitle });

            Board.Assert(b => b.AddPlannedPostCard(postTitle, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString));
        }

}
}
