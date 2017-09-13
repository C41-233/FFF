using FFF.Base.Util.Coroutine;
using FFF.Server.Application.Tick;

namespace FFF.Server.Application.Coroutine
{
    public class WaitForMilliseconds : ICoroutineYield
    {

        private readonly long nextTick;

        public WaitForMilliseconds(long milliseconds)
        {
            this.nextTick = TimeTick.Now.TimeStamp + milliseconds;
        }

        public bool IsYield => TimeTick.Now.TimeStamp < nextTick;

    }
}
