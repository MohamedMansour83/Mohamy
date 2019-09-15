using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.Service.Extensions
{
    public static class Extensions
    {
        public static DateTime ToEgyptTimezone(this DateTime dateOfBirth)
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset usersTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            return usersTime.DateTime;
        }
    }
}
