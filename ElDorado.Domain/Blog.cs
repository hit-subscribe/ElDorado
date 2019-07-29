using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class Blog : IHaveIdentity
    {
        public Blog()
        {

        }
        
        public int Id { get; set; }

        public string Url { get; set; }

        public string CompanyName { get; set; }

        public string FeedlyUrl { get; set; }

        public bool DoWePublish { get; set; }

        public string ClientPostNotes { get; set; }

        public bool IsActive { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogMetric> BlogMetrics { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Whitepaper> Whitepapers { get; set; }

        [NotMapped]
        public string Hostname => new Uri(Url).Host;
    }
}
