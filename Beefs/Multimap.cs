using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public interface Multimap<K, V>
    {
        void Add(K key, V value);
    }
}
