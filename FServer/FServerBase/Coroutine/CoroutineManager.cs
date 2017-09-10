using System;
using System.Collections;
using System.Collections.Generic;
using FFF.Base.Util;
using FFF.Server.Coroutine.Yield;

namespace FFF.Server.Coroutine
{

    internal class CoroutineManager
    {

        private static readonly List<CoroutineContext> coroutines = new List<CoroutineContext>();
        private static readonly List<CoroutineContext> coroutinesToAdd = new List<CoroutineContext>();

        private static readonly Dictionary<ulong, CoroutineContext> idToCoroutine = new Dictionary<ulong, CoroutineContext>();

        internal static CoroutineHandle StartCoroutine(IEnumerator coroutine)
        {
            var context = new CoroutineContext(coroutine);

            coroutinesToAdd.Add(context);
            idToCoroutine[context.Id] = context;

            return context.Handle;
        }

        internal static void StopCoroutine(CoroutineHandle c)
        {
            if (!idToCoroutine.TryGetValue(c.Id, out CoroutineContext coroutine))
            {
                return;
            }
            coroutine.IsDisposed = true;
        }

        internal static void OnTick()
        {
            //add
            {
                coroutines.AddRange(coroutinesToAdd);
                coroutinesToAdd.Clear();
            }

            //do
            {
                for(var i=0; i<coroutines.Count; i++)
                {
                    if (coroutines[i].IsDisposed)
                    {
                        RemoveCoroutineAt(i);
                        i--;
                        continue;
                    }
                    YieldDo(coroutines[i]);
                    if (coroutines[i].IsDisposed)
                    {
                        RemoveCoroutineAt(i);
                        i--;
                    }
                }
            }
        }

        private static void RemoveCoroutineAt(int index)
        {
            var coroutine = coroutines[index];
            coroutines.RemoveAt(index);
            idToCoroutine.Remove(coroutine.Id);
        }

        //执行协程的一帧，凡是yield return的位置，至少要等一帧
        private static void YieldDo(CoroutineContext context)
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
                //协程执行过程中发生异常
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

        }

    }

    internal class CoroutineContext
    {

        private static readonly IdGenerator IdGen = new IdGenerator();

        public CoroutineHandle Handle { get; } = new CoroutineHandle(IdGen.NextValue());
        public ulong Id => Handle.Id;

        private readonly IEnumerator Coroutine;

        public ICoroutineYield Yield;

        public bool IsDisposed
        {
            get => Handle.IsDone;
            set => Handle.IsDone = value;
        }

        public bool MoveNext()
        {
            return Coroutine.MoveNext();
        }

        public object Current => Coroutine.Current;

        public bool IsYield => Yield?.IsYield ?? false;
        public bool IsSuspended => Handle.IsSuspended;

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

    internal class CoroutineHandle : ICoroutine
    {

        public ulong Id { get; }

        public CoroutineHandle(ulong id)
        {
            this.Id = id;
        }

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
            CoroutineManager.StopCoroutine(this);
        }
    }

}
