using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class PostAuthorPairing
    {
        public int Id { get; private set; }
        public string CompanyName { get; private set; }
        public string Title { get; private set; }
        public DateTime? DraftDate { get; private set; }
        public bool IsApproved { get; private set; }
        public bool IsDoublePost { get; private set; }
        public string Authors { get; private set; }

        public PostAuthorPairing(BlogPost post, IEnumerable<Author> authors)
        {
            Id = post.Id;
            CompanyName = post.BlogCompanyName;
            Title = post.Title;
            DraftDate = post.DraftDate;
            IsApproved = post.IsApproved;
            IsDoublePost = post.IsDoublePost;

            Authors = String.Join(", ", authors.Where(a => ShouldAuthorAppearForPost(a, post.DraftDate.Value)).Select(a => $"{a.FirstName} {a.LastName}"));
        }

        private static bool ShouldAuthorAppearForPost(Author author, DateTime date)
        {
            return author.IsActive && author.IsInOurSystems && !author.HasPostsDue(date);
        }
    }
}