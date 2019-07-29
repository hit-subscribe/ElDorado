using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Trello
{
    public class WhitepaperSynchronizer : CalendarService, ITrelloSynchronizer<Whitepaper>
    {
        public WhitepaperSynchronizer(ICalendarBoard board = null)
        {
            Board = board ?? new CalendarBoard("jV76Ii6Z");
        }

        public void CreateCardForEntity(Whitepaper whitepaperToAdd)
        {
            string companyName = whitepaperToAdd.Blog.CompanyName;
            string authorTrelloUserName = whitepaperToAdd.AuthorTrelloUserName;
            
            var card = Board.AddPlannedPostCard(name: whitepaperToAdd.Title, dueDate: whitepaperToAdd.TargetOutlineDate.SafeToMidnightEastern(), trelloUserName: authorTrelloUserName, companyName: companyName);

            whitepaperToAdd.TrelloId = card.Id;
        }

        public void UpdateCardForEntity(Whitepaper whitepaperToEdit)
        {
            var card = Board.AllCards.FirstOrDefault(c => c.Id == whitepaperToEdit?.TrelloId);
            if (card == null)
                return;

            card.DueDate = whitepaperToEdit.TargetOutlineDate.SafeToMidnightEastern();
            card.UpdateLabels(Board.GetLabelsForCompany(whitepaperToEdit.Blog.CompanyName));
            card.UpdateMembers(Board.GetMembersWithUserNames(whitepaperToEdit.AuthorTrelloUserName, whitepaperToEdit.EditorTrelloUserName));
        }
    }
}
