using System.Threading;
using FFF.Base.Util;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForJob : ICoroutineYield
    {
        public bool IsYield { get; private set; } = true;

        public WaitForJob(FAction action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                action();
                IsYield = false;
            });
        }

    }

    public class WaitForJob<T1> : ICoroutineYield
    {

        public bool IsYield { get; private set; } = true;

        public WaitForJob(T1 arg1, FAction<T1> action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                action(arg1);
                IsYield = false;
            });
        }

    }

    public class WaitForJob<T1, T2> : ICoroutineYield
    {

        public bool IsYield { get; private set; } = true;

        public WaitForJob(T1 arg1, T2 arg2, FAction<T1, T2> action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                action(arg1, arg2);
                IsYield = false;
            });
        }

    }
}
