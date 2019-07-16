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
        public void AddCard(PostRefresh refresh)
        {
            Board.AddPlannedPostCard(name: refresh.BlogPost.Title);
        }
    }
}
