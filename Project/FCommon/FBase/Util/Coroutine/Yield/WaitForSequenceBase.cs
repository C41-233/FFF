using FFF.Base.Collection.ReadOnly;

namespace FFF.Base.Util.Coroutine.Yield
{

    internal abstract class WaitForSequenceBase : ICoroutineYieldNeedInit
    {

        public abstract bool IsYield { get; }

        protected IReadOnlyArray<ICoroutineYield> Sequence { get; }

        protected WaitForSequenceBase(ICoroutineYield[] sequence)
        {
            this.Sequence = ReadOnly.Of(sequence);
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
