using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class BlogPost
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        [Required]
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string UrlSlug { get; set; }
        public string Mission { get; set; }
        string Keyword { get; set; }
        public DateTime? DraftDate { get; set; }
        public DateTime? TargetFinalizeDate { get; set; }
        public DateTime? TargetPublicationDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDoublePost { get; set; }
        public string Author { get; set; }
    }
}
