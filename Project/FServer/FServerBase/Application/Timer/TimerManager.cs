using FFF.Base.Util;
using FFF.Server.Application.Tick;
using System.Collections.Generic;
using System.Linq;

namespace FFF.Server.Application.Timer
{
    internal static class TimerManager
    {

        private static readonly SortedList<long, TimerHandle> timers = new SortedList<long, TimerHandle>();

        internal static ITimer StartTimer(long time, FAction callback)
        {
            var timer = new TimerHandle(time, callback);
            timers.Add(time, timer);
            return timer;
        }

        internal static void OnTick()
        {
            while (timers.Count > 0)
            {
                var timer = timers.First().Value;
                if (timer.IsStopped)
                {
                    timers.RemoveAt(0);
                    continue;
                }
                if (timer.Remain > 0)
                {
                    timers.RemoveAt(0);
                    timer.Callback();
                    continue;
                }
                break;
            }
        }

    }

    internal class TimerHandle : ITimer
    {

        private readonly long time;

        public FAction Callback { get; }
        public bool IsStopped { get; private set; }

        public long Remain
        {
            get
            {
                if (IsStopped)
                {
                    return 0;
                }
                var remain = time - TimeTick.NowReal.TimeStamp;
                return remain < 0 ? 0 : remain;
            }
        }

        public TimerHandle(long time, FAction callback)
        {
            this.time = time;
            this.Callback = callback;
        }

        public void Stop()
        {
            this.IsStopped = true;
        }

    }
}
