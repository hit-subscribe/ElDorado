using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.ViewModels
{
    public class BlogPostEditViewModel : BlogPostViewModel
    {

        public decimal? AuthorPay { get; set; }

        public BlogPost Post { get; private set; }

        public BlogPostEditViewModel() : this(new BlogPost(), null) { }

        public BlogPostEditViewModel(BlogPost post, BlogContext context) : base(context)
        {
            Post = post;
            AuthorPay = post.AuthorPay;
            Authors = BuildAuthorsList(context, a => a.IsActive && a.IsInOurSystems);
        }

        public void SetAuthorPay()
        {
            if (AuthorPay == null)
                Post.CalculateAuthorPay();
            else
                Post.AuthorPay = AuthorPay.Value;
        }
    }
}