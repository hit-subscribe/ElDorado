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
    public class When_Evaluating_IsHitSubscribeFinished_BlogPost_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_SubmittedDate_Is_Null()
        {
            var post = new BlogPost();

            post.IsHitSubscribeFinished.ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Post_Has_Submitted_Date()
        {
            var post = new BlogPost() { SubmittedDate = new DateTime(2018, 10, 1) };

            post.IsHitSubscribeFinished.ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_When_Post_Has_SubmittedDate_But_No_PublishDate_And_We_Publish()
        {
            var post = new BlogPost()
            {
                SubmittedDate = new DateTime(2018, 10, 1),
                Blog = new Blog() { DoWePublish = true }
            };

            post.IsHitSubscribeFinished.ShouldBeFalse();
        }
}
}
