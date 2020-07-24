using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Refreshes
{
    public struct SiteUrl
    {
        public string Url { get; set; }

        public DateTime? LastUpdated { get; set; }

        public SiteUrl(string url, DateTime? lastUpdated)
        {
            Url = url;
            LastUpdated = lastUpdated;
        }
    }
}
