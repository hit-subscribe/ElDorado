using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.ViewModelTests
{
    [TestClass]
    public class When_Computing_Quality_Warnings_PostReviewViewModel_Should
    {
        private RenderedPostContents RenderedContents { get; } = Mock.Create<RenderedPostContents>();
        
        private BlogPost Post = new BlogPost()
        {
            Blog = new Blog() { Url = "https://daedtech.com" },
            Title = "Clickbait",
            Content = $@"<p>Imagine an office. Hundreds of employees fill the office. One of the main duties of these employees is to make copies using one of the many copy machines throughout the office. The air smells of toner and the carpet is worn to threads in front of each copy station.</p>{Environment.NewLine}<p>Jerry needs one hundred copies of the <a href=""https://www.shrm.org/resourcesandtools/hr-topics/talent-acquisition/pages/dont-underestimate-the-importance-of-effective-onboarding.aspx"">manual</a> and he can only get them from this <a href=""https://daedtech.com/apage"">Daedtech page</a></p>."
        };

        private PostReviewViewModel Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new PostReviewViewModel(Post, new RenderedPostContents(Post.Content));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Warning_When_No_External_Links_Exist()
        {
            Post.Content = $@"<p>Only an external <a href=""https://wwww.daedtech.com/blah"">link</a>.</p>";

            Target = new PostReviewViewModel(Post, new RenderedPostContents(Post.Content));

            Target.Warnings.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Warning_When_No_Internal_Links_Exist()
        {
            Post.Content = $@"<p>Only an external <a href=""https://wwww.theonion.com/blah"">link</a>.</p>";

            Target = new PostReviewViewModel(Post, new RenderedPostContents(Post.Content));

            Target.Warnings.ShouldNotBeEmpty();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Warning_When_Post_Is_Too_Short()
        {
            Target.Warnings.ShouldContain(s => s.Contains("too short"));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Warning_When_Post_Is_Less_Than_Ninety_Percent_Of_Length()
        {
            RenderedContents.Arrange(rc => rc.WordCount).Returns(1079);

            Target = new PostReviewViewModel(Post, RenderedContents);

            Target.Warnings.ShouldContain(s => s.Contains("too short"));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Warning_When_2x_Post_Is_Less_Than_Ninety_Percent_Of_Length()
        {
            Post.IsDoublePost = true;

            RenderedContents.Arrange(rc => rc.WordCount).Returns(2159);

            Target = new PostReviewViewModel(Post, RenderedContents);

            Target.Warnings.ShouldContain(s => s.Contains("too short"));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_A_Warning_For_Non_SSL_Links()
        {
            Post.Content = $@"<p>Only an external <a href=""http://wwww.theonion.com/blah"">link</a>.</p>";

            Target = new PostReviewViewModel(Post, new RenderedPostContents(Post.Content));

            Target.Warnings.ShouldContain(s => s.Contains("non-SSL links"));
        }
}
}
