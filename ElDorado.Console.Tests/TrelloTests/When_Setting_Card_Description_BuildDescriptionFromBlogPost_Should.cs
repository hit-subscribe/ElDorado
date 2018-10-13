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

namespace ElDorado.Console.Tests.TrelloTests
{
    [TestClass]
    public class When_Setting_Card_Description_BuildDescriptionFromBlogPost_Should
    {
        private ITrelloCard Card { get; set; }

        private readonly BlogPost Post = new BlogPost()
        {
            Mission = "To be a great post",
            Keyword = "great post",
            Blog = new Blog() { ClientPostNotes = "Some Post Notes!"}
        };

        [TestInitialize]
        public void BeforeEachTest()
        {
            Card = Mock.Create<ITrelloCard>();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Mission_And_Colon_To_Description()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain("**Mission**:");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_The_Post_Mission_To_The_Description()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain(Post.Mission);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Keyword_Text_And_BlogPost_Keyword()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain($"**Keyword**: {Post.Keyword}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Persona_Colon_And_Post_Persona()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain($"**Persona**: {Post.Persona}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_ClientNotes_Text_And_Contents()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain($"**Client Notes**: {Post.Blog.ClientPostNotes}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_ClientNotes_Empty_Gracefully_With_Null_Blog()
        {
            Post.Blog = null;

            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain($"**Client Notes**: {string.Empty}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Text_Special_Considerations()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain("**Special Considerations**: ");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Ghostwritten_If_This_Is_A_Ghostwritten_Post()
        {
            Post.IsGhostwritten = true;

            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain("Ghostwritten");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Add_Ghostwritten_When_Post_Is_Not_Ghostwritten()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldNotContain("Ghostwritten");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Double_Post_For_A_Double_Post()
        {
            Post.IsDoublePost = true;

            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain("Double Post");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Add_Double_Post_For_A_Post_Where_IsDouble_Is_False()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldNotContain("Double Post");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Comma_Between_SpecialConsiderations_If_There_Are_Two()
        {
            Post.IsDoublePost = true;
            Post.IsGhostwritten = true;

            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.ShouldContain("Ghostwritten, Double Post");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Newlines_Between_Each_Separate_Token()
        {
            Card.BuildDescriptionFromBlogPost(Post);

            Card.Description.Count(c => c == '\n').ShouldBe(4);
        }
}
}
