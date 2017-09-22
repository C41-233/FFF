using System.Threading;

namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForThread : ICoroutineEventYield
    {
        public bool IsYield { get; private set; } = true;

        public WaitForThread(Thread thread)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                thread.Join();
                IsYield = false;
            });
        }
    }
}
