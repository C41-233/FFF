using FFF.Base.Util;

namespace FFF.Server.Coroutine
{
    public interface ICoroutine
    {

        bool IsDone { get; }

        //仅当协程正常执行结束后会触发
        FAction Callback { get; set; }

        void Suspend();
        void Resume();
        void Stop();

    }
}
