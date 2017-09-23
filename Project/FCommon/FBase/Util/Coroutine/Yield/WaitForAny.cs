using FFF.Base.Linq;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal class WaitForAny : WaitForSequenceBase
    {

        public override bool IsYield => Sequence.Exists(arg => arg.IsYield==false);

        public WaitForAny(ICoroutineYield[] args) : base(args)
        {
        }

    }
}
