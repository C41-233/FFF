using System.Collections;
using FFF.Base.Util;

namespace FFF.Server.Coroutine
{

    public static class FCoroutines
    {

        public static ICoroutine StartCoroutine(IEnumerator coroutine)
        {
            return CoroutineManager.StartCoroutine(coroutine);
        }

        public static ICoroutine StartCoroutine(FFunc<IEnumerator> coroutine)
        {
            return StartCoroutine(coroutine());
        }

    }

}
