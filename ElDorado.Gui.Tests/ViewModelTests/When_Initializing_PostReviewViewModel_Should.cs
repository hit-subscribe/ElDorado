using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Gui.Tests.ViewModelTests
{
    [TestClass]
    public class When_Initializing_PostReviewViewModel_Should
    {
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
            Target = new PostReviewViewModel(Post);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_Word_Count_67_Words()
        {
            Target.WordCount.ShouldBe(67);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_One_Internal_Link()
        {
            Target.InternalLinks.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_No_Internal_Links_When_Post_Blog_Is_Null()
        {
            Post.Blog = null;
            Target = new PostReviewViewModel(Post);
            Target.InternalLinks.Count().ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_One_External_Link()
        {
            Target.ExternalLinks.Count().ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_Internal_Link_With_AnchorText_DaedTech_page()
        {
            Target.InternalLinks.First().AnchorText.ShouldBe("Daedtech page");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_Exception_On_Null_Argument()
        {
            Should.Throw<ArgumentNullException>(() => new PostReviewViewModel(null));	
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Have_Domain_Of_shrm_org_For_External_Link()
        {
            Target.ExternalLinks.First().Domain.ShouldBe("shrm.org");
        }
}
}
