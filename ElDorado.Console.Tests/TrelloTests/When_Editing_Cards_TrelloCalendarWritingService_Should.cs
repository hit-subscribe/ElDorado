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
    public class When_Editing_Cards_TrelloCalendarWritingService_Should
    {
        private IWritingCalendarBoard Board = Mock.Create<IWritingCalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private static readonly string CardId = "ahsdf9";

        private readonly BlogPost Post = new BlogPost()
        {
            Title = "A Blog Post",
            Keyword = "blog",
            DraftDate = new DateTime(2018, 10, 1),
            Blog = new Blog() { CompanyName = "A Company" },
            Author = new Author() { TrelloId = "trello.username" },
            TrelloId = CardId,
            Mission = "Creating havoc"
        };

        private TrelloWritingCalendarService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Card.Arrange(c => c.Id).Returns(CardId);
            Card.Arrange(c => c.ListName).Returns(TrelloWritingCalendarService.PlannedPostsListName);

            Board.Arrange(b => b.AllCards).Returns(new List<ITrelloCard>() { Card });

            Target = new TrelloWritingCalendarService(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Change_The_Name_Of_A_NonMatching_Card()
        {
            Target.EditCard(new BlogPost());

            Card.Assert(c => c.Name, Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_Name_Of_The_Trello_Card_To_Post_AuthorTitle()
        {
            Target.EditCard(Post);

            Mock.AssertSet(() => Card.Name = Post.AuthorTitle, Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_IsArchived_Property_To_True_When_Post_IsApproved_Is_False()
        {
            Target.EditCard(Post);

            Mock.AssertSet(() => Card.IsArchived = true, Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_Card_DueDate_To_Post_DraftDate_At_1159_PM_Eastern()
        {
            Target.EditCard(Post);

            var draftDate = Post.DraftDate.Value;
            var nearMidnight = new DateTime(draftDate.Year, draftDate.Month, draftDate.Day, 23, 59, 59);
            TimeSpan easternTimeUtcOffset = new TimeSpan(-5, 0, 0);
            var nearMidnightOffset = new DateTimeOffset(nearMidnight, easternTimeUtcOffset);

            Mock.AssertSet(() => Card.DueDate = nearMidnightOffset.DateTime, Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Set_DueDate_When_There_Is_No_Draft_Date()
        {
            Post.DraftDate = null;

            Target.EditCard(Post);

            Mock.AssertSet(() => Card.DueDate = Arg.IsAny<DateTime>(), Occurs.Never());
        }

    [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Set_Card_Due_Date_When_Card_Is_In_A_Different_Column()
        {
            Card.Arrange(c => c.ListName).Returns("asdf");

            Target.EditCard(Post);

            Mock.AssertSet(() => Card.DueDate = Arg.IsAny<DateTime>(), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Labels_For_The_Card()
        {
            Target.EditCard(Post);

            Card.Assert(c => c.UpdateLabels(Board.GetLabelsForCompany(Post.BlogCompanyName)), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Update_Labels_In_Other_Columns()
        {
            Card.Arrange(c => c.ListName).Returns("asdf");

            Target.EditCard(Post);

            Card.Assert(c => c.UpdateLabels(Board.GetLabelsForCompany(Post.BlogCompanyName)), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_Members_For_The_Card()
        {
            Target.EditCard(Post);

            Card.Assert(c => c.UpdateMembers(Board.GetMemberWithUserName(Post.AuthorTrelloUserName)), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Remove_Whitespace_When_Checking_For_Trello_Author_Match()
        {
            Post.Author.TrelloId = "erik ";

            Target.EditCard(Post);

            Board.Assert(b => b.GetMemberWithUserName("erik"));
        }

    [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Update_Members_In_Other_Columns()
        {
            Card.Arrange(c => c.ListName).Returns("asdf");

            Target.EditCard(Post);

            Card.Assert(c => c.UpdateMembers(Board.GetMemberWithUserName(Post.AuthorTrelloUserName)), Occurs.Never());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Update_The_Card_Description()
        {
            var dummyCard = Mock.Create<ITrelloCard>();
            dummyCard.BuildDescriptionFromBlogPost(Post);

            Target.EditCard(Post);

            Mock.AssertSet(() => Card.Description = dummyCard.Description, Occurs.Once());
        }
}
}
