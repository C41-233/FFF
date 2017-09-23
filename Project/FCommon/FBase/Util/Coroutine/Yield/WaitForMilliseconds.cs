namespace FFF.Base.Util.Coroutine.Yield
{

    /// <summary>
    /// 等待若干毫秒，此类型将特殊处理
    /// </summary>
    internal class WaitForMilliseconds : CoroutineInitYieldBase
    {

        public long After { get; }

        public WaitForMilliseconds(long after)
        {
            this.After = after;
        }

        public override bool IsYield
        {
            get
            {
                if (IsInit == false)
                {
                    //未经初始化时无法确定当前时间
                    throw new CoroutineException("Cannot check yield of time wait without yield return.");
                }
                return Now < Start + After;
            }
        }
    }
}
