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
    public class When_Evaluating_Whether_Author_Has_Posts_Due_Author_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Author_Post_Collection_Is_Null()
        {
            var author = new Author();

            author.HasPostsDue(new DateTime(2018, 10, 1)).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Author_Has_Posts_With_No_Due_Date()
        {
            var author = new Author() { BlogPosts = new List<BlogPost>() { new BlogPost() } };

            author.HasPostsDue(new DateTime(2018, 9, 1)).ShouldBeFalse();
        }
}
}
