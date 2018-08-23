using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class BlogPost : IHaveIdentity
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public int? AuthorId { get; set; }

        public virtual Author Author { get; set; }

        [Required]
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string UrlSlug { get; set; }
        public string Mission { get; set; }
        public string Keyword { get; set; }
        public DateTime? DraftDate { get; set; }
        public DateTime? TargetFinalizeDate { get; set; }
        public DateTime? TargetPublicationDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDoublePost { get; set; }

        [NotMapped]
        public string AuthorTitle => $"{Title}{(IsDoublePost ? " (2x)" : string.Empty)}";

        [NotMapped]
        public string PostAuthorFirstName => Author?.FirstName;

        [NotMapped]
        public string BlogCompanyName => Blog?.CompanyName;

        public bool IsOlderThan(DateTime target)
        {
            if (TargetPublicationDate != null)
                return TargetPublicationDate < target;
            else if (TargetFinalizeDate != null)
                return TargetFinalizeDate < target;
            else
                return DraftDate < target;
        }
    }
}
