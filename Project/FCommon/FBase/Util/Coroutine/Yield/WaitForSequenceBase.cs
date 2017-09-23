namespace FFF.Base.Util.Coroutine.Yield
{

    internal abstract class WaitForSequenceBase : CoroutineInitYieldBase
    {

        protected ICoroutineYield[] Sequence { get; }

        protected WaitForSequenceBase(ICoroutineYield[] sequence)
        {
            this.Sequence = sequence;
        }

        internal sealed override void Init(CoroutineManager manager)
        {
            base.Init(manager);
            foreach (var arg in Sequence)
            {
                if (arg is CoroutineInitYieldBase yield)
                {
                    yield.Init(manager);
                }
            }
        }

    }
}
