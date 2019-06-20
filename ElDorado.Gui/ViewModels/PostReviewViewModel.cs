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
        public IEnumerable<string> Links { get; } = Enumerable.Empty<string>();


        public PostReviewViewModel(BlogPost post)
        {
            _documentNode = post.Content.AsHtml();

            Title = post.Title;
            WordCount = _documentNode.InnerText.WordCount();

            var linkNodes = _documentNode.SelectNodes("//a[@href]");
            if(linkNodes != null)
                Links = linkNodes.Select(n => n.Attributes["href"]?.Value);
        }
    }
}