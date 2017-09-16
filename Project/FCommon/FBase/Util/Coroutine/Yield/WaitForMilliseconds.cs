namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForMilliseconds : ICoroutineTimerYield
    {

        public long Timeout { get; }

        public WaitForMilliseconds(long milliseconds)
        {
            this.Timeout = milliseconds;
        }

    }
}
