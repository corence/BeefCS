using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.util
{
    public abstract class Chooser
    {
        // ho-leee shit this is the dumbest thing i've ever seen. Why doesn't ICollection implement IReadOnlyCollection? What a terribly clumsy fuckup!
        public T ChooseElement<T>(ICollection<T> collection)
        {
            return ChooseElement(collection, collection.Count);
        }

        public T ChooseElement<T>(IEnumerable<T> collection, int count)
        {
            return ChooseElement(collection.GetEnumerator(), count);
        }

        public T Choose<T>(T a, T b)
        {
            return ChooseElement(new List<T> { a, b });
        }

        public abstract T ChooseElement<T>(IEnumerator<T> enumerator, int count);
    }

    public class LastChooser : Chooser
    {
        public override T ChooseElement<T>(IEnumerator<T> enumerator, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                enumerator.MoveNext();
            }

            return enumerator.Current;
        }
    }

    public class RandomChooser : Chooser
    {
        public readonly Random random;

        public RandomChooser(Random random)
        {
            this.random = random;
        }

        public override T ChooseElement<T>(IEnumerator<T> enumerator, int count)
        {
            int index = random.Next(count);
            int i = 0;
            while (enumerator.MoveNext())
            {
                T element = enumerator.Current;
                if (i == index)
                {
                    return element;
                }
                else
                {
                    ++i;
                }
            }

            throw new Exception("No element found...");
        }
    }
}
