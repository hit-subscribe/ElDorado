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
        private HtmlNode _documentNode; 

        public int WordCount { get; }
        public string Title { get; }
        public IEnumerable<Link> ExternalLinks { get; } = Enumerable.Empty<Link>();

        public IEnumerable<Link> InternalLinks { get; } = Enumerable.Empty<Link>();
        public IList<string> Warnings { get; } = new List<string>();

        public PostReviewViewModel(BlogPost post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _documentNode = post.Content.AsHtml();

            Title = post.Title;
            WordCount = _documentNode.InnerText.WordCount();

            var linkNodes = _documentNode.SelectNodes("//a[@href]");
            var links = linkNodes == null ? Enumerable.Empty<Link>() : linkNodes.Select(ln => new Link(ln));
                
            InternalLinks = links.Where(ln => post.IsInternalLink(ln));
            ExternalLinks = links.Where(ln => !post.IsInternalLink(ln));

            if (!ExternalLinks.Any())
                Warnings.Add("No external links.");
            if (!InternalLinks.Any())
                Warnings.Add("No internal links.");
            if (WordCount < 1000)
                Warnings.Add("Post may be too short.");
        }
    }
}