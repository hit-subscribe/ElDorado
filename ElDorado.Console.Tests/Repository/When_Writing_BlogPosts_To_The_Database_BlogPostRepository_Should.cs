using ElDorado.Domain;
using ElDorado.Exceptions;
using ElDorado.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.DataAccess
{
    [TestClass]
    public class When_Writing_BlogPosts_To_The_Database_BlogPostRepository_Should
    {
        private const string CompanyName = "EvilCorp";

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private Blog BlogInTheDatabase { get; } = new Blog() { CompanyName = CompanyName };

        private Author AuthorInTheDatabase { get; } = new Author() { FirstName = "Erik" };

        private BlogPost BlogPostToAdd { get; } = new BlogPost() { Title = "A Blog Post", Author = new Author() { FirstName = "Erik" }, Blog = new Blog() { CompanyName = CompanyName } };

        private BlogPostRepository Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Context.Blogs.Add(BlogInTheDatabase);
            Context.Authors.Add(AuthorInTheDatabase);

            Target = new BlogPostRepository(Context);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_An_Exception_With_Null_Constructor_Argument()
        {
            Should.Throw<ArgumentNullException>(() => new BlogPostRepository(null));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Save_Changes_To_The_Context()
        {
            Target.Add(BlogPostToAdd.AsEnumerable());

            Context.Assert(ctx => ctx.SaveChanges(), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_Passed_In_BlogPosts()
        {
            Target.Add(BlogPostToAdd.AsEnumerable());

            Context.BlogPosts.ShouldContain(BlogPostToAdd);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Not_Add_A_Blog_Post_That_Already_Has_An_Id()
        {
            BlogPostToAdd.Id = 123;

            Target.Add(BlogPostToAdd.AsEnumerable());

            Context.BlogPosts.ShouldNotContain(BlogPostToAdd);
        }
    
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Match_The_Post_To_A_Blog_By_Matching_CompanyName()
        {
            Target.Add(BlogPostToAdd.AsEnumerable());

            Context.BlogPosts.ShouldContain(bp => bp.Blog == BlogInTheDatabase);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Match_The_Post_To_An_Author_By_Matching_First_Name()
        {
            Target.Add(BlogPostToAdd.AsEnumerable());

            Context.BlogPosts.ShouldContain(bp => bp.Author == AuthorInTheDatabase);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_OrphanedBlogPostException_For_BlogPost_That_Cannot_Be_Matched_To_Blog()
        {
            Should.Throw<OrphanedBlogPostException>(() => Target.Add(new BlogPost() { Blog = new Blog() { CompanyName = "SomeCompany" } }.AsEnumerable()));
        }
    }
}
