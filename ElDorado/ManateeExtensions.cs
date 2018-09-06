using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
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
    }
}
