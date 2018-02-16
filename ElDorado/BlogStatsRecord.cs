using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    public class BlogStatsRecord
    {
        public string BlogName { get; private set; }
        public DateTime Timestamp { get; set; }
        public int SubscriberCount { get; set; }
        public int AlexaRank { get; set; }
        public decimal DomainAuthority { get; set; }
        public int LinkingDomains { get; set; }

        public BlogStatsRecord(string blogRecord)
        {
            BlogName = blogRecord.Split(',')[0];
        }

        public string ToCsv()
        {
            return $"{BlogName},{Timestamp},{SubscriberCount},{AlexaRank},{DomainAuthority},{LinkingDomains}";
        }
    }
}
