using FFF.Base.Util;
using FFF.Base.Util.Coroutine;
using FFF.Base.Util.Coroutine.Yield;
using FFF.Server.Application.Tick;
using System.Collections;

namespace FFF.Server.Application.Coroutine
{

    /// <summary>
    /// 协程
    /// 在逻辑上下文中开启的协程，会在逻辑上下文执行完毕后，同一帧进入
    /// 在协程上下文中开启的协程，会在下一帧进入
    /// 每个yield return至少暂停一帧
    /// 协程基于逻辑时间
    /// </summary>
    public static class Coroutines
    {

        private static readonly CoroutineManager manager = new CoroutineManager();

        public static ICoroutine StartCoroutine(IEnumerator coroutine)
        {
            return manager.StartCoroutine(coroutine);
        }

        public static ICoroutine StartCoroutine(FFunc<IEnumerator> coroutine)
        {
            return StartCoroutine(coroutine());
        }

        public static ICoroutine StartCoroutineAfter(long milliseconds, FAction coroutine)
        {
            return StartCoroutine(CoroutineAfter(milliseconds, coroutine));
        }

        public static IEnumerator CoroutineAfter(long milliseconds, FAction coroutine)
        {
            yield return new WaitForMilliseconds(milliseconds);
            coroutine();
        }

        public static ICoroutine StartCoroutineEvery(long milliseconds, FAction coroutine)
        {
            return StartCoroutine(CoroutineEvery(milliseconds, coroutine));
        }

        public static ICoroutine StartCoroutineEvery(long milliseconds, FFunc<bool> coroutine)
        {
            return StartCoroutine(CoroutineEvery(milliseconds, coroutine));
        }

        public static IEnumerator CoroutineEvery(long milliseconds, FAction coroutine)
        {
            while (true)
            {
                coroutine();
                yield return new WaitForMilliseconds(milliseconds);
            }
        }

        public static IEnumerator CoroutineEvery(long milliseconds, FFunc<bool> coroutine)
        {
            while (true)
            {
                var cont = coroutine();
                if (cont == false)
                {
                    break;
                }
                yield return new WaitForMilliseconds(milliseconds);
            }
        }

        internal static void OnTick(long now)
        {
            manager.Update(now);
        }

    }

}
