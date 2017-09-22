using FFF.Base.Linq;

namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForAny : ICoroutineEventYield
    {

        private readonly ICoroutineEventYield[] args;

        public bool IsYield => args.Exists(arg => arg.IsYield==false);

        public WaitForAny(params ICoroutineEventYield[] args)
        {
            this.args = args;
        }

    }
}
