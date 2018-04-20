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
            var postsToAdd = _planningSpreadsheetService.GetPosts().Where(bp => ShouldBeAddedToTrello(bp));

            foreach (var post in postsToAdd)
                _trelloService.AddCard(post);
        }

        private bool ShouldBeAddedToTrello(BlogPost blogPostToConsider)
        {
            return blogPostToConsider.IsApproved && !_trelloService.DoesCardExist(blogPostToConsider.Title);
        }
    }
}
