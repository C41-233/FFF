using System.Threading;

namespace FFF.Base.Util.Atomic
{
    public class InterlockedBool : IAtomic<bool>
    {

        public InterlockedBool(bool value)
        {
            this.value = Convert(value);
        }

        public bool Value => Convert(this.value);

        private int value;

        public bool Exchange(bool value)
        {
            var target = Convert(value);
            var old = Interlocked.Exchange(ref this.value, target);
            return Convert(old);
        }

        public bool CompareExchange(bool value, bool compare)
        {
            var target = Convert(value);
            var comp = Convert(compare);
            return Interlocked.CompareExchange(ref this.value, target, comp) == comp;
        }

        public static implicit operator bool(InterlockedBool atomic)
        {
            return atomic.Value;
        }

        private static int Convert(bool value)
        {
            return value ? 0 : 1;
        }

        private static bool Convert(int value)
        {
            return value == 0;
        }

    }
}
