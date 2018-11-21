using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado
{
    public static class Extensions
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static IList<IList<object>> Pad(this IList<IList<object>> target, int padding = 0)
        {
            var paddingAmount = padding == 0 ? target.Max(r => r.Count) : padding;

            foreach (var row in target)
            {
                for (int i = row.Count; i < paddingAmount; i++)
                    row.Add(string.Empty);
            }

            return target;
        }

        public static bool TrelloCardLooselyMatches(this string target, string matchCandidate)
        {
            return target.ToLower().Replace("(2x)", string.Empty).Trim() == matchCandidate.ToLower().Replace("  ", " ");
        }

        public static DateTime? SafeToDateTime(this object target)
        {
            return DateTime.TryParse(target?.ToString(), out DateTime result) ? result : (DateTime?)null;
        }
         
        public static DateTime? SafeToMidnightEastern(this DateTime? target)
        {
            if (!target.HasValue)
                return null;

            var draftDate = target.Value;
            var nearMidnight = new DateTime(draftDate.Year, draftDate.Month, draftDate.Day, 23, 59, 59);
            var easternTimeUtcOffset = new TimeSpan(-5, 0, 0);

            return new DateTimeOffset(nearMidnight, easternTimeUtcOffset).DateTime;
        }
    }
}
