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
    public class When_Adding_Cards_To_Trello_RefreshCalendarService_Should
    {
        private const string Title = "Blog Post Title";
        
        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private BlogPost Post { get; set; } = new BlogPost()
        {
            PostRefreshes = new List<PostRefresh>() { new PostRefresh() },
            Title = Title
        };
        
        private RefreshCalendarService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Post.PostRefreshes.First().BlogPost = Post;
            Board.Arrange(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString)).Returns(Card);

            Target = new RefreshCalendarService(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_A_Name_Set_To_BlogPost_Title()
        {
            Target.AddCard(Post.PostRefreshes.First());

            Board.Assert(b => b.AddPlannedPostCard(Post.Title, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString));
        }
}
}
