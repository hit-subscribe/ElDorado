using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui
{
    public static class Extensions
    {
        public static bool ZeroMatches(this int target, int valueToCheck)
        {
            return target == valueToCheck || valueToCheck == 0;
        }

        public static bool ZeroMatches(this int? target, int valueToCheck)
        {
            if (target == null)
                return valueToCheck == 0;

            return ZeroMatches(target.Value, valueToCheck);
        }
    }
}