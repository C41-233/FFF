using FFF.Base.Linq;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForAny : ICoroutineYield
    {

        private readonly ICoroutineYield[] args;

        public bool IsYield => args.Exists(arg => arg.IsYield==false);

        public WaitForAny(params ICoroutineYield[] args)
        {
            this.args = args;
        }

    }
}
