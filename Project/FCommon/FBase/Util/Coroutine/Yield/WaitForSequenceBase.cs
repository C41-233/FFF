namespace FFF.Base.Util.Coroutine.Yield
{

    internal abstract class WaitForSequenceBase : ICoroutineYieldNeedInit
    {

        protected ICoroutineYield[] Sequence { get; }

        public abstract bool IsYield { get; }

        protected WaitForSequenceBase(ICoroutineYield[] sequence)
        {
            this.Sequence = sequence;
        }

        public void Init(ICoroutineTimeGetter time)
        {
            foreach (var arg in Sequence)
            {
                if (arg is ICoroutineYieldNeedInit yield)
                {
                    yield.Init(time);
                }
            }
        }

    }
}
