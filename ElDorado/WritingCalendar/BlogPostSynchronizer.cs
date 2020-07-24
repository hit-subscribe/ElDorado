using ElDorado.Console.Trello;
using ElDorado.Domain;
using System;
using System.Linq;

namespace ElDorado.Console.WritingCalendar
{
    public class BlogPostSynchronizer
    {
        private readonly WritingCalendarService _trelloService;
        private readonly PlanningSpreadsheetService _planningSpreadsheetService;

        public BlogPostSynchronizer(WritingCalendarService trelloService, PlanningSpreadsheetService planningSpreadsheetService)
        {
            _planningSpreadsheetService = planningSpreadsheetService;
            _trelloService = trelloService;
        }

        public void UpdatePlannedInTrello()
        {
            var postsToAdd = _planningSpreadsheetService.GetPosts().Where(bp => ShouldBeAddedToTrello(bp));

            foreach (var post in postsToAdd)
            {
                _trelloService.AddCard(post);
            }
        }

        private bool ShouldBeAddedToTrello(BlogPost blogPostToConsider)
        {
            return blogPostToConsider.IsApproved && !_trelloService.DoesCardExistWithTitle(blogPostToConsider.Title);
        }
    }
}
