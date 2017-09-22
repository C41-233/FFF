using FFF.Base.Collection.PriorityQueue;
using FFF.Base.Linq;
using FFF.Base.Util;
using System;
using System.Collections.Generic;

namespace FFF.Base.Time.Timer
{
    public class TimerManager
    {

        public event Action<ITimer, Exception> OnException;

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
            if (now < 0)
            {
                throw new TimerException("StartTimer before first update.");   
            }
            if (timestamp < now)
            {
                throw new TimerException("Timer timeout before now.");
            }
            var timer = new TimerHandle(this, timestamp, callback);

            var id = timestamp / robinTime;

            var queue = timers.GetValueOrAdd(id);
            queue.Add(timer);
            return timer;
        }

        /// <summary>
        /// Timer更新帧，必须在添加任何Timer之前进行第一次Update
        /// </summary>
        /// <param name="timestamp">当前时间</param>
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
                            OnException?.Invoke(timer, e);
                        }
                        queue.RemoveFirst();
                    }
                    timers.Remove(id);
                }
            }
            {
                //处理当前帧定时器
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
                        if (timer.RemainTime == 0)
                        {
                            timer.Stop();
                            try
                            {
                                timer.Callback();
                            }
                            catch (Exception e)
                            {
                                OnException?.Invoke(timer, e);
                            }
                            queue.RemoveFirst();
                            continue;
                        }
                        break;
                    }
                }
            }
        }

        private class TimerHandle : ITimer, IComparable<TimerHandle>
        {

            private readonly TimerManager manager;

            public FAction Callback { get; }
            public bool IsStopped { get; private set; }
            public string Name { get; set; }

            public long StartTime { get; }
            public long TimeoutTime { get; }
            public long RemainTime
            {
                get
                {
                    if (IsStopped)
                    {
                        return 0;
                    }
                    var remain = TimeoutTime - manager.now;
                    return remain < 0 ? 0 : remain;
                }
            }

            public TimerHandle(TimerManager manager, long timestamp, FAction callback)
            {
                this.manager = manager;
                this.TimeoutTime = timestamp;
                this.Callback = callback;
                this.StartTime = manager.now;
            }

            public void Stop()
            {
                this.IsStopped = true;
            }

            public int CompareTo(TimerHandle other)
            {
                if (this.TimeoutTime == other.TimeoutTime)
                {
                    return 0;
                }
                if (this.TimeoutTime < other.TimeoutTime)
                {
                    return -1;
                }
                return 1;
            }
        }
    }
}
