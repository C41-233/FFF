using System.Collections;
using System.Collections.Generic;

namespace FFF.Base.Collection.ReadOnly
{
    public static class ReadOnly
    {

        private class ReadOnlyArray<T> : IReadOnlyArray<T>
        {

            private readonly T[] array;

            public ReadOnlyArray(T[] array)
            {
                this.array = array;
            }

            public int Length => array.Length;

            public T this[int index] => this.array[index];

            public IEnumerator<T> GetEnumerator()
            {
                return (array as IEnumerable<T>).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return array.GetEnumerator();
            }
        }

        public static IReadOnlyArray<T> Of<T>(T[] array)
        {
            return new ReadOnlyArray<T>(array);
        }

        public static IReadOnlyList<T> Of<T>(List<T> list)
        {
            return list.AsReadOnly();
        }

    }
}
