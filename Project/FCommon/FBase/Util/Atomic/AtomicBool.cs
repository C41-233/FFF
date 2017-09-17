using System.Threading;

namespace FFF.Base.Util.Atomic
{
    public class AtomicBool
    {

        public AtomicBool(bool value)
        {
            this.value = value ? 0 : 1;
        }

        public bool Value => value == 0;

        private int value;

        public bool Exchange(bool value)
        {
            var target = value ? 0 : 1;
            var old = Interlocked.Exchange(ref this.value, target);
            return old == 0;
        }

        public static implicit operator bool(AtomicBool atomic)
        {
            return atomic.Value;
        }

    }
}
