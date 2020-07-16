using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public static class Extensions
    {
        public static int WordCount(this string target)
        {
            return target.Replace("—", " ").Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();
        }

        public static bool MatchesYearAndMonth(this DateTime? target, int year, int month)
        {
            return target.HasValue && target.Value.Year == year && target.Value.Month == month;
        }

        public static string StripLineBreaks(this string target)
        {
            return Regex.Replace(target, @"\r\n?|\n", string.Empty);
        }
    }
}
