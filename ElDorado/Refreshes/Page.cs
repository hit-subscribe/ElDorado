using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Refreshes
{
    public class Page
    {
        private readonly HtmlNode _pageHtml;
        private readonly string _url;

        public string Url => _url;

        public string Title
        {
            get 
            {
                var allTitleNodes = _pageHtml.SelectNodesWithTag("title");
                var title = allTitleNodes.Any() ? allTitleNodes.First().InnerText.ToString() : string.Empty;
                return WebUtility.HtmlDecode(title);
            }
        }

        public string Body
        {
            get
            {
                return _pageHtml.SelectNodesWithTag("body").FirstOrDefault()?.InnerText?.ToString() ?? string.Empty;
            }
        }

        public IEnumerable<string> H1s
        {
            get
            {
                return _pageHtml.SelectNodesWithTag("h1").Select(n => n.InnerText);

            }
        }

        public IEnumerable<string> H2s
        {
            get
            {
                return _pageHtml.SelectNodesWithTag("h2").Select(n => n.InnerText);
            }
        }

        public Page(string rawHtml, string url)
        {
            _url = url;
            _pageHtml = rawHtml.AsHtml();
        }
    }
}
