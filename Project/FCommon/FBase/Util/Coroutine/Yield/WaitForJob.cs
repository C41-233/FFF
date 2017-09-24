using System.Threading;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal sealed class WaitForJob : ICoroutineYield
    {
        public bool IsYield { get; private set; } = true;

        public WaitForJob(FAction action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    action();
                }
                finally
                {
                    IsYield = false;
                }
            });
        }
    }

    internal sealed class WaitForJob<T1> : ICoroutineYield
    {

        public bool IsYield { get; private set; } = true;

        public WaitForJob(T1 arg1, FAction<T1> action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    action(arg1);
                }
                finally
                {
                    IsYield = false;
                }
            });
        }

    }

    internal sealed class WaitForJob<T1, T2> : ICoroutineYield
    {

        public bool IsYield { get; private set; } = true;

        public WaitForJob(T1 arg1, T2 arg2, FAction<T1, T2> action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    action(arg1, arg2);
                }
                finally
                {
                    IsYield = false;
                }
            });
        }

    }
}
