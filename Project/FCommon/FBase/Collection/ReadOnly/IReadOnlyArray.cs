using System.Collections.Generic;

namespace FFF.Base.Collection.ReadOnly
{
    public interface IReadOnlyArray<out T> : IEnumerable<T>
    {

        int Length { get; }

        T this[int index] { get; }

    }
}
