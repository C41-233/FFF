using System;
using FFF.Base.Util;
using System.Collections.Generic;
using System.Linq;

namespace FFF.Base.Time.Timer
{
    public class TimerManager
    {

        private readonly SortedList<long, TimerHandle> timers = new SortedList<long, TimerHandle>();

        private long now;

        public ITimer StartTimer(long timestamp, FAction callback)
        {
            var timer = new TimerHandle(this, timestamp, callback);
            timers.Add(timestamp, timer);
            return timer;
        }

        public void Update(long timestamp)
        {
            this.now = timestamp;

            while (timers.Count > 0)
            {
                var timer = timers.First().Value;
                if (timer.IsStopped)
                {
                    timers.RemoveAt(0);
                    continue;
                }
                if (timer.Remain == 0)
                {
                    timers.RemoveAt(0);
                    try
                    {
                        timer.Callback();
                    }
                    catch (Exception)
                    {
                        //todo 异常处理
                    }
                    continue;
                }
                break;
            }
        }

        private class TimerHandle : ITimer
        {

            private readonly TimerManager manager;

            private readonly long timestamp;

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
                    var remain = timestamp - manager.now;
                    return remain < 0 ? 0 : remain;
                }
            }

            public TimerHandle(TimerManager manager, long timestamp, FAction callback)
            {
                this.manager = manager;
                this.timestamp = timestamp;
                this.Callback = callback;
            }

            public void Stop()
            {
                this.IsStopped = true;
            }

        }
    }

}
