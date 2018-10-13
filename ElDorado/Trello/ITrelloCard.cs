using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.Trello
{
    public interface ITrelloCard
    {
        void Delete();
        void UpdateLabels(IEnumerable<Label> labels);
        void UpdateMembers(IEnumerable<Member> members);

        DateTime? DueDate { get; set; }
        string ListName { get; }
        string Name { get; set; }
        bool? IsArchived { get; set; }
        string Id { get; }
        string Description { get; set; }
    }
}
