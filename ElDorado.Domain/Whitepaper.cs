using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class Whitepaper : IHaveIdentity, IHaveTrelloId
    {
        public Whitepaper()
        {

        }
        
        public int Id { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public int? AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public int? EditorId { get; set; }
        public virtual Editor Editor { get; set; }
        public string Title { get; set; }
        public string Mission { get; set; }
        public string Persona { get; set; }
        public string Notes { get; set; }

        public DateTime? TargetOutlineDate { get; set; }
        public DateTime? OutlineSubmittedDate { get; set; }
        public DateTime? TargetDraftDate { get; set; }
        public DateTime? DraftSubmittedDate { get; set; }
        public DateTime? TargeSubmissionDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public bool IsGhostwritten { get; set; }
        public string TrelloId { get; set; }
        public decimal AuthorPay { get; set; }
        public decimal EditorPay { get; set; }

        [NotMapped]
        public string AuthorTrelloUserName => Author?.TrelloId?.Trim();

        [NotMapped]
        public string EditorTrelloUserName => Editor?.TrelloId?.Trim();

    }
}
