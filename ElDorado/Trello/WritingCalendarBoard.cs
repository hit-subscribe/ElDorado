using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Trello
{
    public class WritingCalendarBoard : IWritingCalendarBoard
    {
        public const string PlannedPostTrelloListName = "Planned Posts";
        public const string TrelloBoardId = "AhqnpUJD";

        private Lazy<Board> LazyWritingCalendar = new Lazy<Board>(() => new Board(TrelloBoardId));
        private Board WritingCalendar => LazyWritingCalendar.Value;
        private CardCollection PlannedPostCards => WritingCalendar.Lists.First(l => l.Name == PlannedPostTrelloListName).Cards;

        public IList<ITrelloCard> AllCards => WritingCalendar.Cards.Filter(CardFilter.All).Select(c => (ITrelloCard)new TrelloCard(c)).ToList();

        public ITrelloCard AddPlannedPostCard(string name, string description = null, DateTime? dueDate = null, string companyName = null, string trelloUserName = null)
        {
            var card = PlannedPostCards.Add(name: name, description: description, dueDate: dueDate, members: GetMembersWithUserNames(trelloUserName), labels: GetLabelsForCompany(companyName));
            return new TrelloCard(card);
        }

        public IEnumerable<Label> GetLabelsForCompany(string companyName)
        {
            return WritingCalendar.Labels.Where(l => l.Name == companyName);
        }

        public IEnumerable<Member> GetMembersWithUserNames(params string[] trelloUserNames)
        {
            return trelloUserNames.Select(tun => WritingCalendar.Members.FirstOrDefault(m => m.UserName == tun)).Where(m => m != null);
        }
    }
}
