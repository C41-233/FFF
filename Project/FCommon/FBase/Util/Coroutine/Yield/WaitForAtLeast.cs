using FFF.Base.Linq;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal class WaitForAtLeast : WaitForSequenceBase
    {

        private readonly uint count;

        public override bool IsYield => Sequence.AtLeast(count, e=>e.IsYield==false);

        public WaitForAtLeast(uint count, ICoroutineYield[] args) : base(args)
        {
            this.count = count;
        }
    }
}
