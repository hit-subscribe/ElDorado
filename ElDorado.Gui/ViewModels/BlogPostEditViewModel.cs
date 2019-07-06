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
        
        public decimal? EditorPay { get; set; }

        public BlogPost Post { get; private set; }

        public BlogPostEditViewModel() : this(new BlogPost(), null) { }

        public BlogPostEditViewModel(BlogPost post, BlogContext context) : base(context)
        {
            Post = post;
            AuthorPay = post.AuthorPay;
            Blogs = BuildClientList(context, b => b.IsActive || post.BlogId == b.Id);
            Authors = BuildAuthorsList(context, a => a.IsActive && a.IsInOurSystems);
        }

        public void SetPay()
        {
            SetAuthorPay();
            SetEditorPay();
        }

        public void SetAuthorPay()
        {
            if (AuthorPay == null)
                Post.CalculateAuthorPay();
            else
                Post.AuthorPay = AuthorPay.Value;
        }

        public void SetEditorPay()
        {
            if (EditorPay == null)
                Post.CalculateEditorPay();
            else
                Post.EditorPay = EditorPay.Value;
        }
    }
}