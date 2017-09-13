using System;

namespace FFF.Base.Time
{
    public static class DateTimeExtension
    {

        public static readonly DateTime UtcStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        public static long ToTimeStamp(this DateTime dt)
        {
            var span = dt.Subtract(UtcStart);
            return (long) span.TotalMilliseconds;
        }

    }
}
