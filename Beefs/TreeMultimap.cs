using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    class TreeMultimap<K, V> : Multimap<K, V>
    {
        private readonly SortedDictionary<K, List<V>> dict;

        public TreeMultimap()
        {
            this.dict = new SortedDictionary<K, List<V>>();
        }

        public void Add(K key, V value)
        {
            List<V> list = dict[key];
            if (list == null)
            {
                list = new List<V>();
                dict.Add(key, list);
            }
            list.Add(value);
        }
    }
}
