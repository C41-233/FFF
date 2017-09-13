using FFF.Base.Time;
using FFF.Base.Util.Coroutine;
using FFF.Server.Application.Tick;

namespace FFF.Server.Application.Coroutine
{
    public class WaitForSeconds : ICoroutineYield
    {

        private readonly long nextTick;

        public WaitForSeconds(long seconds)
        {
            this.nextTick = TimeTick.Now.TimeStamp + seconds * TimeUnit.Second;
        }

        public WaitForSeconds(float seconds)
        {
            this.nextTick = TimeTick.Now.TimeStamp + (long)(seconds * TimeUnit.Second);
        }

        public bool IsYield => TimeTick.Now.TimeStamp < nextTick;

    }
}
