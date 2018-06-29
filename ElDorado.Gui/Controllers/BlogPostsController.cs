﻿using ElDorado.Domain;
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

        public DateTime Today { get; set; } = DateTime.Now;

        public BlogPostsController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public ActionResult Index(int blogId = 0, int authorId = 0, bool includeAll = false)
        {
            var postsFromEvaluatedDatabaseQuery = _blogContext.BlogPosts.ToList(); //I like the readability from these filtering methods we're using, and the cost tradeoff here just doesn't matter

            var filteredPosts = postsFromEvaluatedDatabaseQuery.Where(bp => bp.BlogId.ZeroMatches(blogId) && bp.AuthorId.ZeroMatches(authorId));
            var currentPosts = filteredPosts.Where(bp => includeAll || !bp.IsOlderThan(Today));

            return View(new BlogPostIndexViewModel(currentPosts, _blogContext));
        }

        public ActionResult Create()
        {
            return View(new BlogPostEditViewModel(new BlogPost(), _blogContext));
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
        public ActionResult Edit(BlogPostEditViewModel blogPostViewModel)
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

        private BlogPostEditViewModel GetViewModelForId(int id)
        {
            var blogPost = _blogContext.BlogPosts.First(bp => bp.Id == id);
            return new BlogPostEditViewModel(blogPost, _blogContext);
        }
    }
}