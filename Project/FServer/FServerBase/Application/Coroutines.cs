using System;
using System.Collections;
using FFF.Base.Util;
using FFF.Base.Util.Coroutine;
using FFF.Base.Util.Coroutine.Yield;
using FFF.Server.Application.Tick;

namespace FFF.Server.Application
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

        public static event FAction<ICoroutine, Exception> OnException;

        private static readonly CoroutineManager manager = new CoroutineManager();

        static Coroutines()
        {
            manager.OnException += (c, e) =>
            {
                OnException?.Invoke(c, e);
            };
        }

        public static ICoroutine StartCoroutine(IEnumerator coroutine, string name = null)
        {
            var rst = manager.StartCoroutine(coroutine);
            rst.Name = name;
            return rst;
        }

        public static ICoroutine StartCoroutine(FFunc<IEnumerator> coroutine, string name = null)
        {
            return StartCoroutine(coroutine(), name);
        }

        public static ICoroutine StartCoroutineAfter(long milliseconds, FAction coroutine, string name = null)
        {
            return StartCoroutine(AsCoroutineAfter(milliseconds, coroutine), name);
        }

        public static IEnumerator AsCoroutineAfter(long milliseconds, FAction coroutine)
        {
            yield return WaitFor.Milliseconds(milliseconds);
            coroutine();
        }

        public static ICoroutine StartCoroutineEvery(long milliseconds, FAction coroutine, string name = null)
        {
            return StartCoroutine(AsCoroutineEvery(milliseconds, coroutine), name);
        }

        public static ICoroutine StartCoroutineEvery(long milliseconds, FFunc<bool> coroutine, string name = null)
        {
            return StartCoroutine(AsCoroutineEvery(milliseconds, coroutine), name);
        }

        public static IEnumerator AsCoroutineEvery(long milliseconds, FAction coroutine)
        {
            while (true)
            {
                coroutine();
                yield return WaitFor.Milliseconds(milliseconds);
            }
        }

        public static IEnumerator AsCoroutineEvery(long milliseconds, FFunc<bool> coroutine)
        {
            while (true)
            {
                var cont = coroutine();
                if (cont == false)
                {
                    break;
                }
                yield return WaitFor.Milliseconds(milliseconds);
            }
        }

        internal static void OnTick()
        {
            manager.Update(TimeTick.Now.TimeStamp);
        }

    }

}
