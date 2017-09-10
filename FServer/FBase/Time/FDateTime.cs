using System;
using System.Globalization;

namespace FFF.Base.Time
{

    public class FDateTime
    {

        public static FDateTime Now => new FDateTime(DateTime.Now);

        public static readonly FDateTime UtcStart = DateTimeExtension.UtcStart;

        public static implicit operator FDateTime(DateTime dt)
        {
            return new FDateTime(dt);
        }

        public static implicit operator DateTime(FDateTime fdt)
        {
            return fdt.dt;
        }

        public FDateTime(DateTime dt)
        {
            this.dt = dt;
            this.TimeStamp = dt.ToTimeStamp();
        }

        public FDateTime(long timestamp)
            : this(DateTimeExtension.UtcStart.AddMilliseconds(timestamp))
        {
        }

        public FDateTime(int year, int month, int day, int hour, int minute, int second = 0, int millisecond = 0)
            : this(new DateTime(year, month, day, hour, minute, second, millisecond))
        {
        }

        public FDateTime(int year, int month, int day)
            : this(new DateTime(year, month, day))
        {
        }

        private readonly DateTime dt;

        public long TimeStamp { get; }

        public FDateTime AddMilliseconds(long milliseconds)
        {
            return dt.AddMilliseconds(milliseconds);
        }

        public override string ToString()
        {
            return dt.ToString(CultureInfo.CurrentCulture);
        }

        public override int GetHashCode()
        {
            return dt.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return dt.Equals(obj);
        }
    }

}
