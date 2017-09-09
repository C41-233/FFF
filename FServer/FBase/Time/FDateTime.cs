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
