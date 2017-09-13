using System.Collections;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForCoroutine : ICoroutineYield
    {

        private readonly ICoroutine coroutine;

        public WaitForCoroutine(ICoroutine coroutine)
        {
            this.coroutine = coroutine;
        }

        public WaitForCoroutine(IEnumerator coroutine)
            : this(FCoroutines.StartCoroutine(coroutine))
        {
        }

        public bool IsYield => coroutine.IsDone == false;

    }
}
