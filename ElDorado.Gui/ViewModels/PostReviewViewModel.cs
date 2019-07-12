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
        private readonly RenderedPostContents _contents;

        public int WordCount => _contents.WordCount;
        public string Title { get; }
        public IEnumerable<Link> ExternalLinks => _contents.ExternalLinks;
        public IEnumerable<Link> InternalLinks => _contents.InternalLinks;
        
        public IEnumerable<string> Warnings
        {
            get
            {
                if (!_contents.ExternalLinks.Any())
                    yield return "No external links.";
                if (!_contents.InternalLinks.Any())
                    yield return "No internal links.";
                if (_contents.WordCount < 1000)
                    yield return "Post may be too short.";
            }
        }

        public PostReviewViewModel(BlogPost post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _contents = new RenderedPostContents(post);
            Title = post.Title;
        }
    }
}