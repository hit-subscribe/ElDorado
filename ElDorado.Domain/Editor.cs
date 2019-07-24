using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public class Editor : IHaveIdentity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string TrelloId {get;set;}

        public bool IsActive { get; set; }

        public bool IsInOurSystems { get; set; }

        public decimal BaseRate { get; set; } = 0.04M;

        public int WordpressId { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public bool HasCompletedWorkInMonth(int year, int month)
        {
            return BlogPosts.Any(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month));
        }
    }
}
