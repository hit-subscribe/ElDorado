using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElDorado
{
    public static class Extensions
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static IList<IList<object>> Pad(this IList<IList<object>> target, int padding = 0)
        {
            var paddingAmount = padding == 0 ? target.Max(r => r.Count) : padding;

            foreach (var row in target)
            {
                for (int i = row.Count; i < paddingAmount; i++)
                    row.Add(string.Empty);
            }

            return target;
        }

        public static bool TrelloCardLooselyMatches(this string target, string matchCandidate)
        {
            return target.ToLower().Replace("(2x)", string.Empty).Trim() == matchCandidate.ToLower().Replace("  ", " ");
        }

        public static DateTime? SafeToDateTime(this object target)
        {
            return DateTime.TryParse(target?.ToString(), out DateTime result) ? result : (DateTime?)null;
        }
         
        public static DateTime? SafeToMidnightEastern(this DateTime? target)
        {
            if (!target.HasValue)
                return null;

            var draftDate = target.Value;
            var nearMidnight = new DateTime(draftDate.Year, draftDate.Month, draftDate.Day, 23, 59, 59);
            var easternTimeUtcOffset = new TimeSpan(-5, 0, 0);

            return new DateTimeOffset(nearMidnight, easternTimeUtcOffset).DateTime;
        }

        public static string ToJsonString<T>(this T target)
        {
            return JObject.FromObject(target).ToString();
        }

        public static string ReplaceWeirdQuotes(this string target)
        {
            return target.Replace("“", "\"").Replace("”", "\"").Replace("’", "'").Replace("‘", "'");
        }

        public static HtmlNode AsHtml(this string target)
        {
            var document = new HtmlDocument();
            document.LoadHtml(target);
            return document.DocumentNode;
        }

        public static string SafeGetNodeText(this HtmlNode target, string nodeSequenceString)
        {
            return target.SelectSingleNode(nodeSequenceString)?.InnerText?.ToString() ?? string.Empty;
        }

        public static IEnumerable<string> SafeGetNodeCollectionText(this HtmlNode target, string nodeSequenceString)
        {
            return target.SelectNodes(nodeSequenceString)?.Select(n => n.InnerText) ?? Enumerable.Empty<string>();
        }

        public static IEnumerable<HtmlNode> SelectNodesWithTag(this HtmlNode target, string tag)
        {
            var nodes = target.SelectNodes($"//{tag}");

            return nodes ?? Enumerable.Empty<HtmlNode>();
        }

        public static IEnumerable<string> SelectAttributeValuesForNode(this HtmlNode target, string attribute, string tag)
        {
            return target.SelectNodesWithTag(tag).Select(n => n.Attributes[attribute]?.Value);
        }

        public static IEnumerable<XElement> DescendantsNamed(this XDocument target, string simpleName)
        {
            return target.Descendants().Where(d => d.Name.LocalName == simpleName);
        }

        public static IEnumerable<XElement> DescendantsNamed(this XElement target, string simpleName)
        {
            return target.Descendants().Where(d => d.Name.LocalName == simpleName);
        }

        public static string ValueOfFirstDescendantNamed(this XElement target, string simpleName)
        {
            return DescendantsNamed(target, simpleName).FirstOrDefault()?.Value;
        }

        public static string SafeDomainName(this string targetUrl)
        {
            try
            {
                return new Uri(targetUrl).Host;
            }
            catch(UriFormatException)
            {
                return string.Empty;
            }
        }

        public static bool IsValidUri(this string targetUrl)
        {
            Uri outUri;
            return Uri.TryCreate(targetUrl, UriKind.Absolute, out outUri) && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
