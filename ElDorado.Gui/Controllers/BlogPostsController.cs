using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly BlogContext _blogContext;

        public BlogPostsController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public ActionResult Index(int blogId = 0)
        {
            var matchingPosts = _blogContext.BlogPosts;

            if (blogId == 0)
            {
                return View(new BlogPostIndexViewModel(matchingPosts, _blogContext));
            }
            else
            {
                return View(new BlogPostIndexViewModel(matchingPosts.Where(bp => bp.BlogId == blogId).ToList(), _blogContext));
            }
        }
        public ActionResult Create()
        {
            return View(new BlogPostViewModel(new BlogPost(), _blogContext));
        }

        [HttpPost]
        public ActionResult Create(BlogPost post)
        {
            _blogContext.BlogPosts.Add(post);
            _blogContext.SaveChanges();
            return RedirectToAction("Edit", new { postId = post.Id });
        }

        public ActionResult Edit(int postId)
        {
            return View(GetViewModelForId(postId));
        }


        [HttpPost]
        public ActionResult Edit(BlogPostViewModel blogPostViewModel)
        {
            _blogContext.BlogPosts.Attach(blogPostViewModel.Post);
            _blogContext.SetModified(blogPostViewModel.Post);
            _blogContext.SaveChanges();
            return View(GetViewModelForId(blogPostViewModel.Post.Id));
        }

        public ActionResult Delete(int postId)
        {
            var post = _blogContext.BlogPosts.First(p => p.Id == postId);
            _blogContext.BlogPosts.Remove(post);
            _blogContext.SaveChanges();
            return RedirectToAction("Index");
        }

        private BlogPostViewModel GetViewModelForId(int id)
        {
            var blogPost = _blogContext.BlogPosts.First(bp => bp.Id == id);
            return new BlogPostViewModel(blogPost, _blogContext);
        }
    }
}