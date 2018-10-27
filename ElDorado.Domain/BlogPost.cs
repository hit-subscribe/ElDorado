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
        public string Persona { get; set; }
        public string PostNotes { get; set; }
        public DateTime? DraftDate { get; set; }
        public DateTime? TargetFinalizeDate { get; set; }
        public DateTime? TargetPublicationDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? DraftCompleteDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDoublePost { get; set; }
        public bool IsGhostwritten { get; set; }
        public string TrelloId { get; set; }


        [NotMapped]
        public string AuthorTitle => $"{Title}{(IsDoublePost ? " (2x)" : string.Empty)}";

        [NotMapped]
        public string PostAuthorFirstName => Author?.FirstName;

        [NotMapped]
        public string BlogCompanyName => Blog?.CompanyName;

        [NotMapped]
        public string ClientNotes => Blog?.ClientPostNotes ?? string.Empty;

        [NotMapped]
        public string AuthorTrelloUserName => Author?.TrelloId;

        [NotMapped]
        public bool HasBeenSubmitted => SubmittedDate.HasValue;

        [NotMapped]
        public bool IsHitSubscribeFinished => Blog?.DoWePublish ?? false  ? PublishedDate.HasValue : SubmittedDate.HasValue;
    }
}
