using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain.Tests.AuthorTests
{
    [TestClass]
    public class When_Counting_Eligible_Posts_Author_QualifyingPosts_Should
    {
        private List<BlogPost> AuthorPosts = new List<BlogPost>()
        {
            new BlogPost()
            {
                QualifiesForAuthorBonus = true,
                SubmittedDate = new DateTime(2019, 12, 12)
            }
        };

        private Author Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new Author() { BlogPosts = AuthorPosts };   
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_1_When_Author_Has_A_Single_Qualifying_Post()
        {
            Target.QualifyingPostCount.ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_When_Author_Has_Single_Non_Qualifying_Post()
        {
            AuthorPosts[0].QualifiesForAuthorBonus = false;

            Target.QualifyingPostCount.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_When_Post_Has_Not_Been_Submitted()
        {
            AuthorPosts[0].SubmittedDate = null;

            Target.QualifyingPostCount.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_When_Author_Blog_Posts_Collection_Is_Null()
        {
            Target.BlogPosts = null;

            Target.QualifyingPostCount.ShouldBe(0);
        }
}
}
