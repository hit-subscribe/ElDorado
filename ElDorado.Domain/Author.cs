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
    public class Author : IHaveIdentity
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string BlogUrl { get; set; }
        public string EmailAddress { get; set; }
        public string TrelloId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsInOurSystems { get; set; } = true;
        public decimal BaseRate { get; set; } = 100;
        public int WordpressId { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostRefresh> PostRefreshes { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public bool HasPostsDue(DateTime dueDate)
        {
            return BlogPosts != null && BlogPosts.Any(bp => bp.DraftDate.GetValueOrDefault() == dueDate.Date);
        }

        public bool HasCompletedWorkInMonth(int year, int month)
        {
            return BlogPosts.Any(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)) || 
                PostRefreshes.Any(pr => pr.SubmittedDate.MatchesYearAndMonth(year, month));
        }
    }
}
