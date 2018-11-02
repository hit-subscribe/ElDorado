using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui
{
    public static class Extensions
    {
      
        public static bool DefaultMatches<T>(this T target, T valueToCheck)
        {
            return target.Equals(valueToCheck) || valueToCheck.Equals(default(T));
        }

        public static bool DefaultMatches<T>(this T? target, T valueToCheck) where T : struct
        {
            if (target == null)
                return valueToCheck.Equals(default(T));

            return DefaultMatches(target.Value, valueToCheck);
        }
    }
}