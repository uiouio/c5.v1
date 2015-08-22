using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Extension
{
    public static class DateTimeExtension
    {
        public static long ToTimestamp(this DateTime dateTime)
        {
            var gtm = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            return Convert.ToInt64((dateTime - gtm).TotalSeconds);
        }

        public static DateTime FromTimestamp(this long timestamp)
        {
            var gtm = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return gtm.AddSeconds(timestamp).ToLocalTime();
        }
    }
}
