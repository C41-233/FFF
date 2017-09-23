namespace FFF.Base.Util.Coroutine
{
    internal abstract class CoroutineInitYieldBase : ICoroutineYield
    {

        public abstract bool IsYield { get; }

        private CoroutineManager manager;

        protected long Now
        {
            get
            {
                if (IsInit == false)
                {
                    throw new CoroutineException("Cannot check now without yield return.");
                }
                return manager.Now;
            }
        }

        protected long Start
        {
            get
            {
                if (IsInit == false)
                {
                    throw new CoroutineException("Cannot check start without yield return.");
                }
                return start;
            }
        }

        private long start;

        protected bool IsInit => manager != null;

        internal virtual void Init(CoroutineManager manager)
        {
            if (this.manager != null)
            {
                throw new CoroutineException($"{nameof(ICoroutineYield)} cannot be reused.");
            }
            this.manager = manager;
            this.start = manager.Now;
        }

    }
}