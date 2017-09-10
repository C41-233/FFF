using FFF.Base.Time;

namespace FFF.Server.Application.Time
{
    internal static class TimeTickCache
    {

        static TimeTickCache()
        {
            var now = FDateTime.Now;
            FTimeTick.Start = now;
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
            FTimeTick.Last = FTimeTick.Now;
            FTimeTick.LastReal = FTimeTick.NowReal;
            FTimeTick.Now = logic;
            FTimeTick.NowReal = real;
        }

        internal static void TickTime(long milliseconds)
        {
            TickTime(FDateTime.Now, milliseconds);
        }

        internal static void TickTime(FDateTime real, long milliseconds)
        {
            SetTimeNow(real, FTimeTick.Now.AddMilliseconds(milliseconds));
        }

    }


}
