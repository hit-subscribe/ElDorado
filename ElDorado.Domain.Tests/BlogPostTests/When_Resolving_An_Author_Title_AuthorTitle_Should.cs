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
    public class When_Resolving_An_Author_Title_AuthorTitle_Should
    {
        private const string Title = "The Most Riveting Post Ever";
        private BlogPost Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new BlogPost();
            Target.Title = Title;
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_The_BlogPost_Title_When_Not_A_Double_Post()
        {
            Target.AuthorTitle.ShouldBe(Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_The_BlogPost_Title_Plus_2x_For_DoublePost()
        {
            Target.IsDoublePost = true;

            Target.AuthorTitle.ShouldBe($"{Target.Title} (2x)");
        }
}
}
