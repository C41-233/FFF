using System.Linq;
using FFF.Base.Linq;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForAtLeast : ICoroutineYield
    {

        private readonly ICoroutineYield[] args;
        private readonly uint count;

        public bool IsYield => args.AtLeast(count, e=>e.IsYield==false);

        public WaitForAtLeast(uint count, params ICoroutineYield[] args)
        {
            this.args = args;
            this.count = count;
        }
    }
}
