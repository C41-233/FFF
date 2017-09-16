using FFF.Base.Util.Coroutine.Yield;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FFF.Base.Util.Coroutine
{

    /// <summary>
    /// 时间无关的协程管理
    /// </summary>
    public class CoroutineManager
    {

        private readonly List<CoroutineContext> coroutines = new List<CoroutineContext>();
        private readonly List<CoroutineContext> coroutinesToAdd = new List<CoroutineContext>();

        public ICoroutine StartCoroutine(IEnumerator coroutine)
        {
            var context = new CoroutineContext(this, coroutine);
            coroutinesToAdd.Add(context);
            return context.Handle;
        }

        private long now;

        public void Update(long timestamp)
        {
            this.now = timestamp;

            //add
            {
                coroutines.AddRange(coroutinesToAdd);
                coroutinesToAdd.Clear();
            }

            //do
            for(var i=0; i<coroutines.Count; i++)
            {
                if (coroutines[i].IsDisposed)
                {
                    coroutines.RemoveAt(i);
                    i--;
                    continue;
                }
                YieldDo(coroutines[i]);
                if (coroutines[i].IsDisposed)
                {
                    coroutines.RemoveAt(i);
                    i--;
                }
            }
        }

        //执行协程的一帧，凡是yield return的位置，至少要等一帧
        private void YieldDo(CoroutineContext context)
        {
            //等待
            if (context.IsSuspended)
            {
                return;
            }
            if (context.IsYield)
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
            catch (Exception)
            {
                //todo 协程执行过程中发生异常
                context.IsDisposed = true;
                return;
            }

            var obj = context.Current;

            if (obj is IEnumerator enumerator)
            {
                obj = new WaitForCoroutine(StartCoroutine(enumerator));
            }

            if (obj is ICoroutineYield coroutineYield)
            {
                context.Yield = coroutineYield;
            }
            else if (obj is ICoroutineTimerYield coroutineTimer)
            {
                context.Timestamp = now + coroutineTimer.Timeout;
            }
        }

        private class CoroutineContext
        {

            private readonly CoroutineManager manager;
            public ICoroutine Handle => handle;
            private readonly CoroutineHandle handle = new CoroutineHandle();

            private readonly IEnumerator Coroutine;

            public ICoroutineYield Yield { set; private get; }
            public long Timestamp { set; private get; }

            public bool IsDisposed
            {
                get => handle.IsDone;
                set => handle.IsDone = value;
            }

            public bool MoveNext()
            {
                return Coroutine.MoveNext();
            }

            public object Current => Coroutine.Current;

            public bool IsYield
            {
                get
                {
                    if (manager.now < Timestamp)
                    {
                        return true;
                    }
                    return Yield?.IsYield ?? false;
                }
            } 
            public bool IsSuspended => handle.IsSuspended;

            public CoroutineContext(CoroutineManager manager, IEnumerator c, ICoroutineYield y = null)
            {
                this.manager = manager;
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
