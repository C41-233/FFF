using FFF.Base.Time;
using FFF.Base.Util;
using FFF.Server.Application.TimeTick;

namespace FFF.Server.Timer
{

    /// <summary>
    /// 定时器在指定的时间触发回调函数
    /// ·效率高于协程
    /// ·定时器基于物理时间
    /// </summary>
    public static class FTimers
    {

        public static ITimer StartTimerAfter(ulong milliseconds, FAction callback)
        {
            return TimerManager.StartTimer(
                FTimeTick.NowReal.TimeStamp + (long) milliseconds,
                callback
            );
        }

        public static ITimer StartTimerAt(long timestamp, FAction callback)
        {
            return TimerManager.StartTimer(timestamp, callback);
        }

        public static ITimer StartTimerAt(FDateTime dt, FAction callback)
        {
            return TimerManager.StartTimer(dt.TimeStamp, callback);
        }

    }
    
    public interface ITimer
    {

        long Remain { get; }

        void Stop();

    }
}
