using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Trello
{
    public class RefreshCalendarService : CalendarService
    {
        public RefreshCalendarService(ICalendarBoard board = null)
        {
            Board = board ?? new CalendarBoard("sjMRN6BD");
        }
        public virtual void AddCard(PostRefresh refresh)
        {
            var card = Board.AddPlannedPostCard(
                name: refresh.BlogPost.Title, 
                dueDate: refresh.DraftDate.SafeToMidnightEastern(), 
                trelloUserName: refresh.AuthorTrelloUserName, 
                companyName: refresh.BlogPost.BlogCompanyName
            );

            refresh.TrelloId = card.Id;
        }

        public virtual void EditCard(PostRefresh postRefresh)
        {
            var card = Board.AllCards.FirstOrDefault(c => c.Id == postRefresh?.TrelloId);
            if (card == null)
                return;

            card.DueDate = postRefresh.DraftDate.SafeToMidnightEastern();
            card.UpdateLabels(Board.GetLabelsForCompany(postRefresh.BlogPost.BlogCompanyName));
            card.UpdateMembers(Board.GetMembersWithUserNames(postRefresh.AuthorTrelloUserName));
        }
    }
}
