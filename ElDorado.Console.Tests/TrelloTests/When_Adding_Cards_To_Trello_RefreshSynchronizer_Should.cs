using ElDorado.Domain;
using ElDorado.Trello;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
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
    public class When_Adding_Cards_To_Trello_RefreshSynchronizer_Should
    {
        private const string Title = "Blog Post Title";
        private const string RefreshAuthorTrelloId = "A Trello ID";
        private const string CompanyName = "Some Company";


        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private BlogPost Post { get; set; } = new BlogPost()
        {
            PostRefreshes = new List<PostRefresh>()
            {
                new PostRefresh()
                {
                    DraftDate = new DateTime(2019, 7, 4),
                    Author = new Author()
                    {
                        TrelloId = RefreshAuthorTrelloId
                    }
                }
            },
            Title = Title,
            Blog = new Blog()
            {
                CompanyName = CompanyName
            }
        };

        private PostRefresh Refresh => Post.PostRefreshes.First();
        
        private RefreshSynchronizer Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Refresh.BlogPost = Post;
            Board.Arrange(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString)).Returns(Card);

            Target = new RefreshSynchronizer(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_A_Name_Set_To_BlogPost_Title()
        {
            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Post.Title, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_DueDate_Set_To_DraftDate_Plus_12_Hours()
        {
            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Refresh.DraftDate.SafeToMidnightEastern(), Arg.AnyString, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_TrelloUserName_Set_To_Post_AuthorTrelloUserName()
        {
            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, RefreshAuthorTrelloId));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Null_TrelloId_When_No_Refresh_Author_Is_Specified()
        {
            Refresh.Author = null;

            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.IsAny<string[]>()));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Null_TrelloId_When_TrelloId_Is_Null()
        {
            Refresh.Author.TrelloId = null;

            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.IsAny<string[]>()));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_TrelloId_Trimmed_For_Whitespace()
        {
            Refresh.Author.TrelloId = RefreshAuthorTrelloId + " ";

            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, RefreshAuthorTrelloId));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_CompanyName_Set_To_Post_CompanyName()
        {
            Target.CreateCardForEntity(Refresh);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Post.BlogCompanyName, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Post_Trello_Id_Based_On_Card_TrelloId()
        {
            const string trelloId = "asdf";
            Card.Arrange(c => c.Id).Returns(trelloId);

            Target.CreateCardForEntity(Refresh);

            Refresh.TrelloId.ShouldBe(trelloId);
        }

    }
}
