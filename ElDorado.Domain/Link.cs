using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            AnchorText = ComputeAnchorText(node);
            Url = node.Attributes["href"].Value;
            Domain = ComputeDomain(Url);
        }

        public Link(string simpleUrl)
        {
            Url = simpleUrl;
            Domain = ComputeDomain(Url);
        }

        private static string ComputeAnchorText(HtmlNode node)
        {
            if (string.IsNullOrEmpty(node.InnerText) && !node.ChildNodes.Any())
                return string.Empty;
            else if (string.IsNullOrEmpty(node.InnerText))
                return node.ChildNodes.First().Name + " tag";

            return node.InnerText;
        }

        private static string ComputeDomain(string url)
        {
            var fullHost = new Uri(url).Host;
            var tokens = fullHost.Split('.').Reverse().ToList();
            return tokens.Count > 1 ? $"{tokens[1]}.{tokens[0]}" : tokens[0];
        }
    }
}
