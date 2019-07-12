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
        private readonly BlogPost _post;
        private readonly RenderedPostContents _contents;

        public string Title => _post.Title;
        public IEnumerable<Link> ExternalLinks => _contents.Links.Where(ln => !_post.IsInternalLink(ln));
        public IEnumerable<Link> InternalLinks => _contents.Links.Where(ln => _post.IsInternalLink(ln));
        
        public IEnumerable<string> Warnings
        {
            get
            {
                if (!ExternalLinks.Any())
                    yield return "No external links.";
                if (!InternalLinks.Any())
                    yield return "No internal links.";
                if (_contents.WordCount < _post.TargetWordCount * .9M) 
                    yield return "Post may be too short.";
                if (_contents.Links.Any(l => l.Url.Contains("http://")))
                    yield return "Post contains non-SSL links.";
            }
        }

        public Dictionary<string, string> Stats = new Dictionary<string, string>();

        public PostReviewViewModel(BlogPost post, RenderedPostContents postContents)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _post = post;
            _contents = postContents;

            Stats["Word Count"] = _contents.WordCount.ToString();
            Stats["Paragraph Count"] =_contents.Paragraphs.Count().ToString();
            Stats["Sentences Per Paragraph"] = _contents.Paragraphs.Count() == 0 ? string.Empty : ((decimal)_contents.Sentences.Count() / (decimal)_contents.Paragraphs.Count()).ToString();
        }
    }
}