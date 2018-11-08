using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class AuthorTimelinessRecord
    {
        public int PostCount { get; private set; }
        public string Name { get; private set; }
        public string Timeliness { get; private set; }
        public int TotalPosts { get; private set; }
        public int LatePosts { get; private set; }

        public AuthorTimelinessRecord(Author author)
        {
            PostCount = author.BlogPosts != null ? author.BlogPosts.Count(bp => bp.DraftCompleteDate.HasValue) : 0;
            Name = $"{author.FirstName} {author.LastName}";
            Timeliness = GetEligiblePosts(author).Any() ? FormatTimeliness(author) : string.Empty;
            TotalPosts = GetEligiblePosts(author).Count();
            LatePosts = author.BlogPosts != null ? author.BlogPosts.Count(bp => bp.DraftCompleteDate > bp.DraftDate) : 0;
        }

        private string FormatTimeliness(Author author)
        {
            var earliness = CalculateTimeliness(author);
            var earlinessDescriptor = earliness >= 0 ? "Early" : "Late";
            return $"{Math.Abs(earliness).ToString("0.##")} Days {earlinessDescriptor}";
        }

        private double CalculateTimeliness(Author author)
        {
            return GetEligiblePosts(author).Average(bp => (bp.DraftDate.Value - bp.DraftCompleteDate.Value).TotalDays);
        }

        private IEnumerable<BlogPost> GetEligiblePosts(Author author)
        {
            return author.BlogPosts.Where(bp => bp.DraftDate.HasValue && bp.DraftCompleteDate.HasValue);
        }
    }
}