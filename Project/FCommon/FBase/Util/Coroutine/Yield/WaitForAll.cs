using System.Linq;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal class WaitForAll : WaitForSequenceBase
    {

        public override bool IsYield => Sequence.All(arg => arg.IsYield == false);

        public WaitForAll(ICoroutineYield[] args) : base(args)
        {
        }

    }
}
