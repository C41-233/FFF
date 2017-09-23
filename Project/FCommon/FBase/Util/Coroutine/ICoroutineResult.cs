namespace FFF.Base.Util.Coroutine
{

    public interface ICoroutineResult<out T>
    {

        T Result { get; }

    }

}
