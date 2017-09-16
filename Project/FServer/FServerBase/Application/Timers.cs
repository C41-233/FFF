﻿using FFF.Base.Time;
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

        public static ITimer StartTimer(long milliseconds, FAction callback)
        {
            return manager.StartTimer(
                TimeTick.NowReal.TimeStamp + milliseconds,
                callback
            );
        }

        public static ITimer StartTimerTimeoutAt(long timestamp, FAction callback)
        {
            return manager.StartTimer(timestamp, callback);
        }

        public static ITimer StartTimerTimeoutAt(FDateTime dt, FAction callback)
        {
            return manager.StartTimer(dt.TimeStamp, callback);
        }

        internal static void OnTick(long now)
        {
            manager.Update(now);
        }

    }
    
}