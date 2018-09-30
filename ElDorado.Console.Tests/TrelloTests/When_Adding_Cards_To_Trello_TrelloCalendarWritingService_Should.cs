﻿using ElDorado.Domain;
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
    public class When_Adding_Cards_To_Trello_TrelloCalendarWritingService_Should
    {
        private IWritingCalendarBoard Board = Mock.Create<IWritingCalendarBoard>();

        private ITrelloCard Card = Mock.Create<ITrelloCard>();

        private readonly BlogPost Post = new BlogPost()
        {
            Title = "Some Blog Post",
            DraftDate = new DateTime(2018, 10, 1),
            Blog = new Blog() { CompanyName = "Some Company"},
            Author = new Author() { TrelloId = "ah98sfh"},
            Keyword = "asdf",
            Mission = "To boldly go..."
        };

        private TrelloWritingCalendarService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Board.Arrange(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString)).Returns(Card);

            Target = new TrelloWritingCalendarService(Board);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_Name_Set_To_Post_AuthorTitle()
        {
            Target.AddCard(Post);

            Board.Assert(b => b.AddPlannedPostCard(Post.Title, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_DueDate_Set_To_DraftDate_Plus_12_Hours()
        {
            Target.AddCard(Post);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Post.DraftDate.SafeAddHours(12), Arg.AnyString, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_TrelloUserName_Set_To_Post_AuthorTrelloUserName()
        {
            Target.AddCard(Post);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Arg.AnyString, Post.AuthorTrelloUserName));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Card_With_CompanyName_Set_To_Post_CompanyName()
        {
            Target.AddCard(Post);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Arg.AnyString, Arg.IsAny<DateTime?>(), Post.BlogCompanyName, Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_Card_Keyword()
        {
            Target.AddCard(Post);

            Card.Assert(c => c.SetKeyword(Post.Keyword));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Is_Archived_To_False_When_Post_IsApproved()
        {
            Post.IsApproved = true;

            Target.AddCard(Post);

            Mock.AssertSet(() => Card.IsArchived = false, Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Post_Trello_Id_Based_On_Card_TrelloId()
        {
            const string trelloId = "asdf";
            Card.Arrange(c => c.Id).Returns(trelloId);

            Target.AddCard(Post);

            Post.TrelloId.ShouldBe(trelloId);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Description_To_Be_Post_Mission()
        {
            Target.AddCard(Post);

            Board.Assert(b => b.AddPlannedPostCard(Arg.AnyString, Post.Mission, Arg.IsAny<DateTime?>(), Post.BlogCompanyName, Arg.AnyString));
        }
}
}
