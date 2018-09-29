using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Trello
{
    public static class ManateeExtensions
    {
        public static void UpdateMembers(this Card target, IEnumerable<Member> members)
        {
            foreach (var member in target.Members.ToList())
                target.Members.Remove(member);

            foreach (var member in members)
                target.Members.Add(member);
        }

        public static void UpdateLabels(this Card target, IEnumerable<Label> labels)
        {
            foreach (var label in target.Labels.ToList())
                target.Labels.Remove(label);

            foreach (var label in labels)
                target.Labels.Add(label);
        }

        public static void SetKeyword(this Card target, string keyword)
        {
            string commentText = $"Keyword: {keyword}";

            var keywordComment = target.Comments.FirstOrDefault(c => c.Data != null && c.Data.Text != null && c.Data.Text.Contains("Keyword"));

            if (keywordComment != null && keywordComment.Data != null)
                keywordComment.Data.Text = commentText;
            else
                target.Comments.Add(commentText);
        }
    }
}
