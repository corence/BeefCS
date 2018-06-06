using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.util
{
    public class CombinedEnumerator<T> : IEnumerator<T>
    {
        public readonly IEnumerator<IEnumerator<T>> enumeratorEnumerator;

        public CombinedEnumerator(IEnumerator<IEnumerator<T>> enumeratorEnumerator)
        {
            this.enumeratorEnumerator = enumeratorEnumerator;
        }

        public CombinedEnumerator(IEnumerator<T> a, IEnumerator<T> b)
        {
            enumeratorEnumerator = new List<IEnumerator<T>> { a, b }.GetEnumerator();
        }

        public T Current => enumeratorEnumerator.Current.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            do
            {
                enumeratorEnumerator.Current.Dispose();
            } while (enumeratorEnumerator.MoveNext());

            enumeratorEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (enumeratorEnumerator.Current.MoveNext())
                {
                    return true;
                }

                if (!enumeratorEnumerator.MoveNext())
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
