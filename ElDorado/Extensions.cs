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
    }
}
