using FFF.Base.Linq;

namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForAtLeast : ICoroutineEventYield
    {

        private readonly ICoroutineEventYield[] args;
        private readonly uint count;

        public bool IsYield => args.AtLeast(count, e=>e.IsYield==false);

        public WaitForAtLeast(uint count, params ICoroutineEventYield[] args)
        {
            this.args = args;
            this.count = count;
        }
    }
}
