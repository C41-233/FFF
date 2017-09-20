using System.Collections.Generic;

namespace FFF.Base.Linq
{
    public static class DictionaryExtension
    {

        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dic, K key)
        {
            return dic.TryGetValue(key, out V value) ? value : default(V);
        }

        public static V GetValueOrAddDefault<K, V>(this IDictionary<K, V> dic, K key)
        {
            if (!dic.TryGetValue(key, out V value))
            {
                value = dic[key] = default(V);
            }
            return value;
        }

        public static V GetValueOrAdd<K, V>(this IDictionary<K, V> dic, K key)
            where V : new()
        {
            if (!dic.TryGetValue(key, out V value))
            {
                value = dic[key] = new V();
            }
            return value;
        }

    }
}
