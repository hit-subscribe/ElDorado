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
        private readonly HtmlNode _documentNode;

        public virtual IEnumerable<Link> Links { get; }

        public virtual int WordCount => _documentNode.InnerText.WordCount();

        public virtual IEnumerable<string> Paragraphs => _documentNode.SelectNodesWithTag("p").Select(node => node.InnerHtml);

        public virtual IEnumerable<string> Sentences => Paragraphs.SelectMany(p => p.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries));

        public RenderedPostContents(string rawContents)
        {
            _documentNode = rawContents.AsHtml();

            var linkNodes = _documentNode.SelectNodes("//a[@href]");
            Links = linkNodes == null ? Enumerable.Empty<Link>() : linkNodes.Select(ln => new Link(ln));
        }
    }
}