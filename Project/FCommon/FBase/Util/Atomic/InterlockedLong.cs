using System.Threading;

namespace FFF.Base.Util.Atomic
{
    public class InterlockedLong : Atomic<long>
    {

        public override long Value => value;

        private long value;

        public InterlockedLong(long value)
        {
            this.value = value;
        }

        public override long Exchange(long value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }

        public override bool CompareExchange(long value, long comp)
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

    }
}
