using FFF.Base.Time;

namespace FFF.Base.Util.Coroutine.Yield
{
    public class WaitForSeconds : ICoroutineTimerYield
    {

        public long Timeout { get; }

        public WaitForSeconds(long seconds)
        {
            this.Timeout = seconds * TimeUnit.Second;
        }

        public WaitForSeconds(float seconds)
        {
            this.Timeout = (long)(seconds * TimeUnit.Second);
        }

    }
}
