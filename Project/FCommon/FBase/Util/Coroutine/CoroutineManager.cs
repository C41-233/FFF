using FFF.Base.Time.Timer;
using FFF.Base.Util.Coroutine.Yield;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FFF.Base.Util.Coroutine
{

    /// <summary>
    /// 协程
    /// </summary>
    public class CoroutineManager
    {

        public event FAction<ICoroutine, Exception> OnException;

        private readonly List<CoroutineContext> coroutines = new List<CoroutineContext>();
        private readonly TimerManager timerManager = new TimerManager();

        public ICoroutine StartCoroutine(IEnumerator coroutine)
        {
            var context = new CoroutineContext(coroutine);
            coroutines.Add(context);
            return context.Handle;
        }

        internal long Now { get; private set; }

        public void Update(long timestamp)
        {
            this.Now = timestamp;

            //do timer 必须在event之前进行，以保证定时器到期后立刻恢复执行协程
            {
                timerManager.Update(Now);
            }

            //do event
            {
                var slot = 0;
                for(var i=0; i<coroutines.Count; i++)
                {
                    var coroutine = coroutines[i];
                    if (coroutine.IsDisposed || coroutine.IsInTimer)
                    {
                        continue;
                    }

                    YieldDo(coroutine);

                    if (coroutine.IsDisposed || coroutine.IsInTimer)
                    {
                        continue;
                    }

                    coroutines[slot] = coroutine;
                    slot++;
                }

                coroutines.RemoveRange(slot, coroutines.Count-slot);
            }

        }

        //执行协程的一帧，凡是yield return的位置，至少要等一帧
        private void YieldDo(CoroutineContext context)
        {
            //主动暂停
            if (context.IsSuspended)
            {
                return;
            }
            //等待事件
            if (context.IsWaitForEvent)
            {
                return;
            }

            context.Yield = null;

            try
            {
                //当前协程执行完毕
                if (!context.MoveNext())
                {
                    context.IsDisposed = true;
                    context.Callback();
                    return;
                }
            }
            catch (Exception e)
            {
                context.IsDisposed = true;
                try
                {
                    OnException?.Invoke(context.Handle, e);
                }
                catch (Exception)
                {
                    //todo 处理异常     
                }
                return;
            }

            var obj = context.Current;

            if (obj is IEnumerator enumerator)
            {
                obj = new WaitForCoroutine(StartCoroutine(enumerator));
            }

            if (obj is CoroutineInitYieldBase yield)
            {
                yield.Init(this);
            }

            if (obj is WaitForMilliseconds coroutineTimer)
            {
                context.IsInTimer = true;
                timerManager.StartTimer(Now + coroutineTimer.After, () =>
                {
                    context.IsInTimer = false;
                    coroutines.Add(context);
                });

                //这里return，不作为ICoroutineYield处理了
                return;
            }

            if (obj is ICoroutineYield coroutineYield)
            {
                context.Yield = coroutineYield;
            }

        }

        private class CoroutineContext
        {

            public ICoroutine Handle => handle;
            private readonly CoroutineHandle handle = new CoroutineHandle();

            private readonly IEnumerator Coroutine;

            public ICoroutineYield Yield { set; private get; }

            public bool IsDisposed
            {
                get => handle.IsDone;
                set => handle.IsDone = value;
            }

            public bool IsInTimer { get; set; }

            public bool MoveNext()
            {
                return Coroutine.MoveNext();
            }

            public object Current => Coroutine.Current;

            public bool IsWaitForEvent => Yield?.IsYield ?? false;
            public bool IsSuspended => handle.IsSuspended;

            public CoroutineContext(IEnumerator c, ICoroutineYield y = null)
            {
                this.Coroutine = c;
                this.Yield = y;
            }

            public void Callback()
            {
                Handle.Callback?.Invoke();
            }
        }

    }

    internal class CoroutineHandle : ICoroutine
    {

        public bool IsDone { get; set; } = false;
        public bool IsSuspended { get; private set; } = false;

        public string Name { get; set; }
        public FAction Callback { get; set; }

        public void Suspend()
        {
            IsSuspended = true;
        }

        public void Resume()
        {
            IsSuspended = false;
        }

        public void Stop()
        {
            IsDone = true;
        }
    }

}
