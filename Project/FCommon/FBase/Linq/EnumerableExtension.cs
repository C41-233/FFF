using System;
using System.Collections.Generic;
using System.Linq;

namespace FFF.Base.Linq
{
    public static class EnumerableExtension
    {

        public static bool Exists<T>(this IEnumerable<T> source, Predicate<T> pred)
        {
            return source.Any(e => pred(e));
        }

        public static bool NotExists<T>(this IEnumerable<T> source, Predicate<T> pred)
        {
            return source.Any(e => pred(e)) == false;
        }

        public static bool AtLeast<T>(this IEnumerable<T> source, uint count, Predicate<T> pred)
        {
            if (count == 0)
            {
                return true;
            }
            uint current = 0;
            foreach (var e in source)
            {
                if (pred(e))
                {
                    current++;
                }
                if (current == count)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
