using ElDorado.Domain;
using ElDorado.Trello;
using Manatee.Trello;
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
    public class When_Editing_Cards_RefreshSynchronizer_Should
    {
        private const string Title = "Blog Post Title";
        private const string RefreshAuthorTrelloId = "A Trello ID";
        private const string CompanyName = "Some Company";
        private const string CardTrelloId = "asdfh";
        
        private ICalendarBoard Board = Mock.Create<ICalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private BlogPost Post { get; set; } = new BlogPost()
        {
            PostRefreshes = new List<PostRefresh>()
            {
                new PostRefresh()
                {
                    DraftDate = new DateTime(2019, 7, 4),
                    TrelloId = CardTrelloId,
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

            Card.Arrange(c => c.Id).Returns(CardTrelloId);
            Card.Arrange(c => c.ListName).Returns(CalendarService.PlannedPostsListName);

            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { Card });

            Target = new RefreshSynchronizer(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Change_The_Name_Of_A_Non_Matching_Card()
        {
            Target.UpdateCardForEntity(new PostRefresh());

            Card.Assert(c => c.Name, Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_Card_Due_Date_To_Refresh_DraftDate()
        {
            Target.UpdateCardForEntity(Refresh);

            var draftDate = Refresh.DraftDate.Value;
            var nearMidnight = new DateTime(draftDate.Year, draftDate.Month, draftDate.Day, 23, 59, 59);
            TimeSpan easternTimeUtcOffset = new TimeSpan(-5, 0, 0);
            var nearMidnightOffset = new DateTimeOffset(nearMidnight, easternTimeUtcOffset);

            Mock.AssertSet(() => Card.DueDate = nearMidnightOffset.DateTime, Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Set_DueDate_When_There_Is_No_Draft_Date()
        {
            Refresh.DraftDate = null;

            Target.UpdateCardForEntity(Refresh);

            Mock.AssertSet(() => Card.DueDate = Arg.IsAny<DateTime>(), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Labels_For_The_Card()
        {
            Target.UpdateCardForEntity(Refresh);

            Card.Assert(c => c.UpdateLabels(Board.GetLabelsForCompany(Refresh.BlogPost.BlogCompanyName)), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Members_For_The_Card()
        {
            var members = new List<Member>() { null };
            Board.Arrange(b => b.GetMembersWithUserNames(Refresh.AuthorTrelloUserName)).Returns(members);

            Target.UpdateCardForEntity(Refresh);

            Card.Assert(c => c.UpdateMembers(members), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Remove_Whitespace_When_Checking_For_Trello_Author_Match()
        {
            Refresh.Author.TrelloId = "erik ";

            Target.UpdateCardForEntity(Refresh);

            Board.Assert(b => b.GetMembersWithUserNames("erik"));
        }

    }
}
