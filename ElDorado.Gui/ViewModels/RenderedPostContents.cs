using ElDorado.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class RenderedPostContents
    {
        private BlogPost _post;
        private HtmlNode _documentNode;
        private IEnumerable<Link> _links;

        public IEnumerable<Link> ExternalLinks => _links.Where(ln => !_post.IsInternalLink(ln));
        public IEnumerable<Link> InternalLinks => _links.Where(ln => _post.IsInternalLink(ln));

        public int WordCount => _documentNode.InnerText.WordCount();

        public RenderedPostContents(BlogPost post)
        {
            _post = post;
            _documentNode = post.Content.AsHtml();
            var linkNodes = _documentNode.SelectNodes("//a[@href]");
            _links = linkNodes == null ? Enumerable.Empty<Link>() : linkNodes.Select(ln => new Link(ln));
        }
    }
}