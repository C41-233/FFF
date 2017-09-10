using System.Threading;

namespace FFF.Server.Application.Time
{
    internal class TimeFrame
    {

        private readonly long realTick;
        private readonly long logicTick;

        private long nextFrame;

        internal TimeFrame(ulong realTick, ulong logicTick)
        {
            this.realTick = (long) realTick;
            this.logicTick = (long) logicTick;
            TimeTickCache.SetTimeNow();

            var currentFrame = FTimeTick.NowReal.TimeStamp;
            nextFrame = currentFrame + this.realTick;
        }

        internal void JoinFrame()
        {
            var currentFrame = FTimeTick.NowReal.TimeStamp;
            if (nextFrame > currentFrame)
            {
                Thread.CurrentThread.Join((int)(nextFrame - currentFrame));
            }

            TimeTickCache.TickTime(logicTick);

            nextFrame += realTick;
        }

    }
}
