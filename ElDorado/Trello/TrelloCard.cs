using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElDorado.Console.Trello;
using Manatee.Trello;

namespace ElDorado.Console.Trello
{
    public class TrelloCard : ITrelloCard
    {
        private Card _card;

        public string Id => _card.Id;

        public bool? IsArchived
        {
            get => _card.IsArchived;
            set => _card.IsArchived = value;
        }

        public string Name
        {
            get => _card.Name;
            set => _card.Name = value;
        }

        public string ListName => _card.List.Name;

        public DateTime? DueDate
        {
            get => _card.DueDate;
            set => _card.DueDate = value;
        }

        public string Description
        {
            get => _card.Description;
            set => _card.Description = value;
        }

        public TrelloCard(Card card)
        {
            _card = card;
        }

        public void UpdateMembers(IEnumerable<Member> members)
        {
            foreach (var member in _card.Members.ToList())
                _card.Members.Remove(member);

            foreach (var member in members)
                _card.Members.Add(member);
        }

        public void UpdateLabels(IEnumerable<Label> labels)
        {
            foreach (var label in _card.Labels.ToList())
                _card.Labels.Remove(label);

            foreach (var label in labels)
                _card.Labels.Add(label);
        }

        public void Delete() => _card.Delete();
    }
}
