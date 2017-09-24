using System.IO;
using System.Threading;
using FFF.Base.Linq;
using FFF.Base.Time;
using FFF.Base.Util.Coroutine.Yield;

namespace FFF.Base.Util.Coroutine
{
    public static class WaitFor
    {

        public static ICoroutineYield All(params ICoroutineYield[] args)
        {
            return new WaitForAll(args);
        }

        public static ICoroutineYield Any(params ICoroutineYield[] args)
        {
            return new WaitForAny(args);
        }

        public static ICoroutineYield AtLeast(uint count, params ICoroutineYield[] args)
        {
            return new WaitForAtLeast(count, args);
        }

        public static ICoroutineYield Coroutine(ICoroutine coroutine)
        {
            return new WaitForCoroutine(coroutine);
        }

        public static ICoroutineYield Job(FAction action)
        {
            return new WaitForJob(action);
        }

        public static ICoroutineYield Job<T1>(T1 arg1, FAction<T1> action)
        {
            return new WaitForJob<T1>(arg1, action);
        }

        public static ICoroutineYield Job<T1, T2>(T1 arg1, T2 arg2, FAction<T1, T2> action)
        {
            return new WaitForJob<T1, T2>(arg1, arg2, action);
        }

        public static ICoroutineYield Read(Stream stream, byte[] buffer, out ICoroutineResult<int> result)
        {
            return Read(stream, buffer, 0, buffer.Length, out result);
        }

        public static ICoroutineYield Read(Stream stream, byte[] buffer, int offset, int len, out ICoroutineResult<int> result)
        {
            return F.Assign(out result, new WaitForReadBuffer(stream, buffer, offset, len));
        }

        public static ICoroutineYield Read(Stream stream, int len, out ICoroutineResult<byte[]> result)
        {
            return F.Assign(out result, new WaitForRead(stream, len));
        }

        public static ICoroutineYield Thread(Thread thread)
        {
            return new WaitForThread(thread);
        }

        public static ICoroutineYield Milliseconds(long milliseconds)
        {
            return new WaitForMilliseconds(milliseconds);
        }

        public static ICoroutineYield Seconds(long seconds)
        {
            return new WaitForMilliseconds(seconds * TimeUnit.Second);
        }

        public static ICoroutineYield Seconds(float seconds)
        {
            return new WaitForMilliseconds((long)(seconds * TimeUnit.Second));
        }

    }
}
