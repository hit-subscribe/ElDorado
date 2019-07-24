using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui
{
    public static class Extensions
    {
      
        public static bool DefaultMatches<T>(this T target, T valueToCheck)
        {
            return target.Equals(valueToCheck) || valueToCheck.Equals(default(T));
        }

        public static bool DefaultMatches<T>(this T? target, T valueToCheck) where T : struct
        {
            if (target == null)
                return valueToCheck.Equals(default(T));

            return DefaultMatches(target.Value, valueToCheck);
        }

        public static HtmlNode AsHtml(this string target)
        {
            var document = new HtmlDocument();
            document.LoadHtml(target);
            return document.DocumentNode;
        }

        public static IEnumerable<HtmlNode> SelectNodesWithTag(this HtmlNode target, string tag)
        {
            var nodes = target.SelectNodes($"//{tag}");

            return nodes ?? Enumerable.Empty<HtmlNode>();
        }
    }
}