using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.Trello
{
    public interface ITrelloCard
    {
        void Delete();
        void UpdateLabels(IEnumerable<Manatee.Trello.Label> labels);
        void UpdateMembers(IEnumerable<Manatee.Trello.Member> members);
        void SetKeyword(string keyword);

        DateTime? DueDate { get; set; }
        string ListName { get; }
        string Name { get; set; }
        bool? IsArchived { get; set; }
        string Id { get; }
    }
}
