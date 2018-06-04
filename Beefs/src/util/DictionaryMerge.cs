using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.util
{
    public class DictionaryMerge
    {
        public static Dictionary<K, V> Merge<K, V>(
            IReadOnlyDictionary<K, V> lhs,
            IReadOnlyDictionary<K, V> rhs,
            Func<V, V> lhsFunc,
            Func<V, V, V> mergeFunc,
            Func<V, V> rhsFunc)
        {
            return Merge(lhs, rhs.AsEnumerable<KeyValuePair<K, V>>().GetEnumerator(), lhsFunc, mergeFunc, rhsFunc);
        }

        public static Dictionary<K, V> Merge<K, V>(
            IReadOnlyDictionary<K, V> lhs,
            IEnumerator<KeyValuePair<K, V>> rhs,
            Func<V, V> lhsFunc,
            Func<V, V, V> mergeFunc,
            Func<V, V> rhsFunc)
        {
            Dictionary<K, V> result = new Dictionary<K, V>();

            while (rhs.MoveNext())
            {
                var entry = rhs.Current;
                if (lhs.ContainsKey(entry.Key))
                {
                    V value = mergeFunc(lhs[entry.Key], entry.Value);

                    if (value != null)
                    {
                        result[entry.Key] = value;
                    }
                }
                else
                {
                    V value = rhsFunc(entry.Value);

                    if (value != null)
                    {
                        result[entry.Key] = value;
                    }
                }
            }

            foreach (var entry in lhs)
            {
                if (!result.ContainsKey(entry.Key))
                {
                    V value = lhsFunc(entry.Value);
                    if (value != null)
                    {
                        result[entry.Key] = value;
                    }
                }
            }

            return result;
        }

        public static Func<V, V> Identity<V>()
        {
            return a => a;
        }
    }
}
