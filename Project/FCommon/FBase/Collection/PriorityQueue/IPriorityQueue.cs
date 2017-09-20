namespace FFF.Base.Collection.PriorityQueue
{
    public interface IPriorityQueue<T>
    {

        int Count { get; }

        void Add(T value);

        T First { get; }

        T RemoveFirst();

    }

    public static class PriorityQueueExtension
    {

        public static void Add<T>(this IPriorityQueue<T> queue, params T[] values)
        {
            foreach (var value in values)
            {
                queue.Add(value);
            }
        }

    }
}
