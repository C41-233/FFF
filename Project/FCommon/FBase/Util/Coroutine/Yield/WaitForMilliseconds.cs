namespace FFF.Base.Util.Coroutine.Yield
{

    /// <summary>
    /// 等待若干毫秒，此类型将特殊处理
    /// </summary>
    internal sealed class WaitForMilliseconds : ICoroutineYieldNeedInit
    {

        public long After { get; }

        private ICoroutineTimeGetter time;

        private long Start;

        private long Now => time.Now;

        public WaitForMilliseconds(long after)
        {
            this.After = after;
        }

        public bool IsYield
        {
            get
            {
                if (time == null)
                {
                    //未经初始化时无法确定当前时间
                    throw new CoroutineException("Cannot check yield of time wait without yield return.");
                }
                return Now < Start + After;
            }
        }

        public void Init(ICoroutineTimeGetter time)
        {
            if (this.time != null)
            {
                throw new CoroutineException("Cannnot reuse yield.");
            }
            this.time = time;
            this.Start = time.Now;
        }
    }
}
