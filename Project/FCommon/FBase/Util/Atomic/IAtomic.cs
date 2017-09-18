namespace FFF.Base.Util.Atomic
{
    public interface IAtomic<T>
    {

        T Value { get; }

        T Exchange(T value);

        bool CompareExchange(T value, T comp);

    }
}
