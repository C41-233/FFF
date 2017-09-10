using FFF.Server.Application.Time;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForMilliseconds : ICoroutineYield
    {

        private readonly long nextTick;

        public WaitForMilliseconds(long milliseconds)
        {
            this.nextTick = FTimeTick.Now.TimeStamp + milliseconds;
        }

        public bool IsYield => FTimeTick.Now.TimeStamp < nextTick;

    }
}
