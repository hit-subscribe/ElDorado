using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElDorado.Domain;
using HtmlAgilityPack;

namespace ElDorado.Gui.ViewModels
{
    public class PostReviewViewModel
    {
        private BlogPost _post;

        public int WordCount { get; set; }
        public string Title { get; set; }

        public PostReviewViewModel(BlogPost post)
        {
            _post = post;
            Title = post.Title;
            SetWordCount();
        }

        private void SetWordCount()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_post.Content);

            var parsedContent = htmlDocument.DocumentNode.InnerText;
            WordCount = parsedContent.WordCount();
        }
    }
}