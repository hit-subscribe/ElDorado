﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manatee.Trello;

namespace ElDorado.Trello
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

        public TrelloCard(Card card)
        {
            _card = card;
        }

        public void SetKeyword(string keyword)
        {
            string commentText = $"Keyword: {keyword}";

            var keywordComment = _card.Comments.FirstOrDefault(c => c.Data != null && c.Data.Text != null && c.Data.Text.Contains("Keyword"));

            if (keywordComment != null && keywordComment.Data != null)
                keywordComment.Data.Text = commentText;
            else
                _card.Comments.Add(commentText);
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