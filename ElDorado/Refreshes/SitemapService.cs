using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElDorado.Console.Refreshes
{
    public class SitemapService
    {
        public virtual IEnumerable<SiteUrl> GetSiteUrlsFromSitemap(string sitemapUrl)
        {
            var document = XDocument.Load(sitemapUrl);

            return document.DescendantsNamed("url").Select(n =>
                new SiteUrl(n.ValueOfFirstDescendantNamed("loc"), n.ValueOfFirstDescendantNamed("lastmod").SafeToDateTime())
            );
        }

        public virtual IEnumerable<string> GetBasicUrlsFromSitemap(string sitemapUrl)
        {
            return GetSiteUrlsFromSitemap(sitemapUrl).Select(surl => surl.Url);
        }
    }
}
