using System.Linq;

namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForAll : ICoroutineEventYield
    {

        private readonly ICoroutineEventYield[] args;

        public bool IsYield => args.All(arg => arg.IsYield == false);

        public WaitForAll(params ICoroutineEventYield[] args)
        {
            this.args = args;
        }

    }
}
