using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElDorado.Refreshes
{
    public class Sitemap
    {
        private readonly XDocument _document;

        public virtual IEnumerable<SiteUrl> SiteUrls => _document.DescendantsNamed("url").Select(n => 
        new SiteUrl(n.ValueOfFirstDescendantNamed("loc"), n.ValueOfFirstDescendantNamed("lastmod").SafeToDateTime())
        );

        public virtual IEnumerable<string> Urls => SiteUrls.Select(su => su.Url);

        public Sitemap(string xmlFilePath, XDocument document = null)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                throw new ArgumentNullException(nameof(xmlFilePath));

            _document = document ?? XDocument.Load(xmlFilePath);
        }

        public Sitemap(XDocument document)
        {
            _document = document;
        }
    }
}
