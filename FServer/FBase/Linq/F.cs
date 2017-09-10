using System.Collections.Generic;

namespace FFF.Base.Linq
{
    public static class F
    {

        public static IEnumerable<int> For<T>(List<T> list)
        {
            var i = 0;
            while (i < list.Count)
            {
                yield return i++;
            }
        }

        public static IEnumerable<int> For(int count)
        {
            var i = 0;
            while (i < count)
            {
                yield return i++;
            }
        }

    }
}
