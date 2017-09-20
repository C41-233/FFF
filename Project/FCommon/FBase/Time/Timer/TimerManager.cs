using FFF.Base.Collection.PriorityQueue;
using FFF.Base.Linq;
using FFF.Base.Util;
using System;
using System.Collections.Generic;

namespace FFF.Base.Time.Timer
{
    public class TimerManager
    {

        private readonly Dictionary<long, PriorityQueue<TimerHandle>> timers = new Dictionary<long, PriorityQueue<TimerHandle>>();

        private readonly long robinTime;

        private long now = -1;

        public TimerManager()
            : this(1000)
        {
        }

        /// <summary>
        /// 创建TimerManager实例
        /// </summary>
        /// <param name="roundTime">每roundTime为一组</param>
        public TimerManager(long roundTime)
        {
            if (roundTime <= 0)
            {
                throw new ArgumentException($"{nameof(roundTime)}={roundTime}");
            }
            this.robinTime = roundTime;
        }

        public ITimer StartTimer(long timestamp, FAction callback)
        {
            var timer = new TimerHandle(this, timestamp, callback);

            var id = timestamp / robinTime;

            var queue = timers.GetValueOrAdd(id);
            queue.Add(timer);
            return timer;
        }

        public void Update(long timestamp)
        {
            if (now < 0)
            {
                now = timestamp;
            }

            var lastId = now / robinTime;
            var thisId = timestamp / robinTime;

            this.now = timestamp;

            //处理已过期定时器
            for(var id = lastId; id < thisId; id++)
            {
                if (timers.TryGetValue(id, out PriorityQueue<TimerHandle> queue))
                {
                    while (queue.Count > 0)
                    {
                        var timer = queue.First;
                        if (timer.IsStopped)
                        {
                            queue.RemoveFirst();
                            continue;
                        }

                        timer.Stop();
                        try
                        {
                            timer.Callback();
                        }
                        catch (Exception e)
                        {
                            OnException(timer, e);
                        }
                        queue.RemoveFirst();
                    }
                    timers.Remove(id);
                }
            }
            {
                if (timers.TryGetValue(thisId, out PriorityQueue<TimerHandle> queue))
                {
                    while (queue.Count > 0)
                    {
                        var timer = queue.First;
                        if (timer.IsStopped)
                        {
                            queue.RemoveFirst();
                            continue;
                        }
                        if (timer.Remain == 0)
                        {
                            timer.Stop();
                            try
                            {
                                timer.Callback();
                            }
                            catch (Exception e)
                            {
                                OnException(timer, e);
                            }
                            queue.RemoveFirst();
                            continue;
                        }
                        break;
                    }
                }
            }
        }

        private void OnException(TimerHandle timer, Exception e)
        {
            //todo
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            if (e == null) throw new ArgumentNullException(nameof(e));
        }

        private class TimerHandle : ITimer, IComparable<TimerHandle>
        {

            private readonly TimerManager manager;

            public FAction Callback { get; }
            public bool IsStopped { get; private set; }

            public long Timeout { get; }

            public long Remain
            {
                get
                {
                    if (IsStopped)
                    {
                        return 0;
                    }
                    var remain = Timeout - manager.now;
                    return remain < 0 ? 0 : remain;
                }
            }

            public TimerHandle(TimerManager manager, long timestamp, FAction callback)
            {
                this.manager = manager;
                this.Timeout = timestamp;
                this.Callback = callback;
            }

            public void Stop()
            {
                this.IsStopped = true;
            }

            public int CompareTo(TimerHandle other)
            {
                if (this.Timeout == other.Timeout)
                {
                    return 0;
                }
                if (this.Timeout < other.Timeout)
                {
                    return -1;
                }
                return 1;
            }
        }
    }
}
