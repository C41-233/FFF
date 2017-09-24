using System;

namespace FFF.Base.Util.Coroutine
{

    public interface ICoroutineResult<out T>
    {

        T Value { get; }

        Exception Exception { get; }

    }

}
