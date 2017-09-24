using System.Threading;

namespace FFF.Base.Util.Atomic
{
    public sealed class InterlockedBool : Atomic<bool>
    {

        public InterlockedBool(bool value)
        {
            this.value = Convert(value);
        }

        public override bool Value => Convert(this.value);

        private int value;

        public override bool Exchange(bool value)
        {
            var target = Convert(value);
            var old = Interlocked.Exchange(ref this.value, target);
            return Convert(old);
        }

        public override bool CompareExchange(bool value, bool compare)
        {
            var target = Convert(value);
            var comp = Convert(compare);
            return Interlocked.CompareExchange(ref this.value, target, comp) == comp;
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
