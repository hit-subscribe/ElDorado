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

        public BlogPostsController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public ActionResult Index()
        {
            return View(_blogContext.BlogPosts);
        }
        public ActionResult Edit(int id)
        {
            return View(GetViewModelForId(id));
        }

        [HttpPost]
        public ActionResult Edit(BlogPostViewModel blogPostViewModel)
        {
            _blogContext.BlogPosts.Attach(blogPostViewModel.Post);
            _blogContext.SetModified(blogPostViewModel.Post);
            _blogContext.SaveChanges();
            return View(GetViewModelForId(blogPostViewModel.Post.Id));
        }

        private BlogPostViewModel GetViewModelForId(int id)
        {
            var blogPost = _blogContext.BlogPosts.First(bp => bp.Id == id);
            return new BlogPostViewModel(blogPost, _blogContext);
        }
    }
}