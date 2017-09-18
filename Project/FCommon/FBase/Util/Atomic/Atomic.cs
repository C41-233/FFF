namespace FFF.Base.Util.Atomic
{
    public abstract class Atomic<T>
    {

        public abstract T Value { get; }

        public abstract T Exchange(T value);

        public abstract bool CompareExchange(T value, T comp);

        public static implicit operator T(Atomic<T> atomic)
        {
            return atomic.Value;
        }

    }
}
