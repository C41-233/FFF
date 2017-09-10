using FFF.Base.Time;
using FFF.Server.Application.Time;

namespace FFF.Server.Coroutine.Yield
{
    public class WaitForSeconds : ICoroutineYield
    {

        private readonly long nextTick;

        public WaitForSeconds(long seconds)
        {
            this.nextTick = FTimeTick.Now.TimeStamp + seconds * TimeUnit.Second;
        }

        public WaitForSeconds(float seconds)
        {
            this.nextTick = FTimeTick.Now.TimeStamp + (long)(seconds * TimeUnit.Second);
        }

        public bool IsYield => FTimeTick.Now.TimeStamp < nextTick;

    }
}
