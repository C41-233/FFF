namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForCoroutine : ICoroutineEventYield
    {

        private readonly ICoroutine coroutine;

        public WaitForCoroutine(ICoroutine coroutine)
        {
            this.coroutine = coroutine;
        }

        public bool IsYield => coroutine.IsDone == false;

    }
}
