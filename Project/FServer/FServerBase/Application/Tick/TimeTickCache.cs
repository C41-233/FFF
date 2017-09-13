using FFF.Base.Time;

namespace FFF.Server.Application.Tick
{
    internal static class TimeTickCache
    {

        static TimeTickCache()
        {
            var now = FDateTime.Now;
            TimeTick.Start = now;
            SetTimeNow(now);
        }

        internal static void SetTimeNow()
        {
            SetTimeNow(FDateTime.Now);
        }

        internal static void SetTimeNow(FDateTime dt)
        {
            SetTimeNow(dt, dt);
        }

        internal static void SetTimeNow(FDateTime real, FDateTime logic)
        {
            TimeTick.Last = TimeTick.Now;
            TimeTick.LastReal = TimeTick.NowReal;
            TimeTick.Now = logic;
            TimeTick.NowReal = real;
        }

        internal static void TickTime(long milliseconds)
        {
            TickTime(FDateTime.Now, milliseconds);
        }

        internal static void TickTime(FDateTime real, long milliseconds)
        {
            SetTimeNow(real, TimeTick.Now.AddMilliseconds(milliseconds));
        }

    }


}
