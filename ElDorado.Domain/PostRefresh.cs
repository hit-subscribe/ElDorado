using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class PostRefresh : IHaveIdentity
    {
        public PostRefresh()
        {

        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public int? AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public DateTime? DraftDate { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public DateTime? TargetPublicationDate { get; set; }

        public DateTime? Published { get; set; }

        public string TrelloId { get; set; }

        public decimal AuthorPay { get; set; } = 50;

        [NotMapped]
        public string AuthorTrelloUserName => Author?.TrelloId?.Trim();
    }
}
