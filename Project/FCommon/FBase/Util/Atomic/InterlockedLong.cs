using System.Threading;

namespace FFF.Base.Util.Atomic
{
    public class InterlockedLong : IAtomic<long>
    {

        public long Value => value;

        private long value;

        public InterlockedLong(long value)
        {
            this.value = value;
        }

        public long Exchange(long value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }

        public bool CompareExchange(long value, long comp)
        {
            return Interlocked.CompareExchange(ref this.value, value, comp) == comp;
        }

        public long Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        public long Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        public static implicit operator long(InterlockedLong atomic)
        {
            return atomic.value;
        }

    }
}
