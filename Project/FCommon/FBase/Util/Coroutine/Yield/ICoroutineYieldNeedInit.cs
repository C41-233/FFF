namespace FFF.Base.Util.Coroutine.Yield
{
    internal interface ICoroutineYieldNeedInit : ICoroutineYield
    {

        void Init(ICoroutineTimeGetter time);

    }
}
