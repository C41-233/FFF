namespace FFF.Base.Collection.PriorityQueue
{
    public interface IPriorityQueue<T>
    {

        int Count { get; }

        void Add(T value);

        T First { get; }

        T RemoveFirst();

    }
}
