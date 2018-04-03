using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.WritingCalendar
{
    public class BlogPostSynchronizer
    {
        private readonly TrelloWritingCalendarService _trelloService;
        private readonly PlanningSpreadsheetService _planningSpreadsheetService;
        public BlogPostSynchronizer(TrelloWritingCalendarService trelloService, PlanningSpreadsheetService planningSpreadsheetService)
        {
            _planningSpreadsheetService = planningSpreadsheetService;
            _trelloService = trelloService;
        }

        public void UpdatePlannedInTrello()
        {
            var allPlannedPosts = _planningSpreadsheetService.GetPlannedPosts();
            var postsToAdd = allPlannedPosts.Where(bp => ShouldBeAddedToTrello(bp)).ToList();

            foreach (var post in postsToAdd)
                _trelloService.AddCard(post.Title);
        }

        private bool ShouldBeAddedToTrello(BlogPost blogPostToConsider)
        {
            return blogPostToConsider.IsApproved && !_trelloService.DoesCardExist(blogPostToConsider.Title);
        }
    }
}
