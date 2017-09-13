using System.Linq;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForAll : ICoroutineYield
    {

        private readonly ICoroutineYield[] args;

        public bool IsYield => args.All(arg => arg.IsYield == false);

        public WaitForAll(params ICoroutineYield[] args)
        {
            this.args = args;
        }

    }
}
