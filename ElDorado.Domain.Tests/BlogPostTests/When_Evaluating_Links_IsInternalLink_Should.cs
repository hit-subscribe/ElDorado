using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain.Tests.BlogPostTests
{
    [TestClass]
    public class When_Evaluating_Links_IsInternalLink_Should
    {
        BlogPost Target = new BlogPost();

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Blog_Is_Null()
        {
            Target.IsInternalLink(new Link("https://daedtech.com")).ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Blog_Url_Matches_Target_Url()
        {
            Target.Blog = new Blog() { Url = "https://daedtech.com" };

            Target.IsInternalLink(new Link("https://daedtech.com/book")).ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Blog_Url_Is_Null()
        {
            Target.Blog = new Blog();

            Target.IsInternalLink(new Link("https://daedtech.com/book")).ShouldBeFalse();
        }
}
}
