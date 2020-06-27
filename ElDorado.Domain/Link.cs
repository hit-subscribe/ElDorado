using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class Link
    {
        public string AnchorText { get; } = string.Empty;

        public string Url { get; }

        public string Domain { get; }

        public Link(HtmlNode node)
        {
            AnchorText = string.IsNullOrEmpty(node.InnerText) ? node.ChildNodes.First().Name + " tag" : node.InnerText;
            Url = node.Attributes["href"].Value;
            Domain = ComputeDomain(Url);
        }

        public Link(string simpleUrl)
        {
            Url = simpleUrl;
            Domain = ComputeDomain(Url);
        }

        private static string ComputeDomain(string url)
        {
            var fullHost = new Uri(url).Host;
            var tokens = fullHost.Split('.').Reverse().ToList();
            return tokens.Count > 1 ? $"{tokens[1]}.{tokens[0]}" : tokens[0];
        }
    }
}
