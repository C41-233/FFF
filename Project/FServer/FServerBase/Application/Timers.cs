using FFF.Base.Time;
using FFF.Base.Time.Timer;
using FFF.Base.Util;
using FFF.Server.Application.Tick;

namespace FFF.Server.Application
{

    /// <summary>
    /// 定时器在指定的时间触发回调函数
    /// ·效率高于协程
    /// ·定时器基于物理时间
    /// </summary>
    public static class Timers
    {

        private static readonly TimerManager manager = new TimerManager();

        public static ITimer StartTimer(long milliseconds, FAction callback, string name = null)
        {
            return StartTimerTimeoutAt(
                TimeTick.NowReal.TimeStamp + milliseconds,
                callback,
                name
            );
        }

        public static ITimer StartTimerTimeoutAt(long timestamp, FAction callback, string name = null)
        {
            var timer = manager.StartTimer(timestamp, callback);
            timer.Name = name;
            return timer;
        }

        public static ITimer StartTimerTimeoutAt(FDateTime dt, FAction callback, string name = null)
        {
            return StartTimerTimeoutAt(dt.TimeStamp, callback, name);
        }

        internal static void OnInit()
        {
            manager.Update(TimeTick.Now.TimeStamp);
        }

        internal static void OnTick()
        {
            manager.Update(TimeTick.Now.TimeStamp);
        }

    }
    
}
