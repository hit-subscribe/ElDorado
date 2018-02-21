using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class BlogMetric
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public DateTime Recorded { get; set; }

        public int FeedlySubscribers { get; set; }

        public int AlexaRanking { get; set; }

        public decimal DomainAuthority { get; set; }

        public int LinkingRootDomains { get; set; }
    }
}
