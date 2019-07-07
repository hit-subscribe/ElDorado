using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.Trello
{
    public interface IWritingCalendarBoard
    {
        IEnumerable<Label> GetLabelsForCompany(string companyName);

        IEnumerable<Member> GetMembersWithUserNames(params string[] trelloUserNames);

        IList<ITrelloCard> AllCards { get; }

        ITrelloCard AddPlannedPostCard(string name = null, string description = null, DateTime? dueDate = null, string companyName = null, string trelloUserName = null);
    }
}
