﻿using Beefs.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class OptimizingTask : Task
    {
        public readonly Task implementation;
        public readonly IReadOnlyDictionary<string, TaskCluster> possibilities;

        public OptimizingTask(Task implementation, IReadOnlyDictionary<string, TaskCluster> possibilities)
            : base("optimize with " + implementation.name,
                   implementation.needs,
                   implementation.positions,
                   CombineValues(implementation.outcomes, CombineClusterOutcomes(possibilities.Values)))
        {
            this.implementation = implementation;
            this.possibilities = possibilities;
        }

        public static OptimizingTask Merge(OptimizingTask a, OptimizingTask b, Chooser chooser)
        {
            if (a.name != b.name)
            {
                throw new Exception("can't merge these optimizing tasks");
            }

            Dictionary<string, TaskCluster> mergedPossibilities = DictionaryMerge.Merge(
                a.possibilities,
                b.possibilities,
                DictionaryMerge.Identity<TaskCluster>(),
                (x, y) => new TaskCluster(x, y, chooser),
                DictionaryMerge.Identity<TaskCluster>());

            Task implementation = chooser.Choose(a.implementation, b.implementation);
            return new OptimizingTask(a.implementation, mergedPossibilities);
        }

        public int NumPossibilities()
        {
            return possibilities.Values.Select(cluster => cluster.count).Sum();
        }

        public static IReadOnlyDictionary<K, double> CombineValues<K>(IReadOnlyDictionary<K, double> values1, IReadOnlyDictionary<K, double> values2)
        {
            return MakeDict(new CombinedEnumerator<KeyValuePair<K, double>>(values1.GetEnumerator(), values2.GetEnumerator()));
        }

        public static Dictionary<K, V> MakeDict<K, V>(IEnumerator<KeyValuePair<K, V>> enumerator)
        {
            Dictionary<K, V> result = new Dictionary<K, V>();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Current;
                result.Add(entry.Key, entry.Value);
            }
            return result;
        }

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
                    if(enumeratorEnumerator.Current.MoveNext())
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

        public static IReadOnlyDictionary<Resource, double> CombineClusterOutcomes(IEnumerable<TaskCluster> clusters)
        {
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>();

            foreach (var dict in clusters.Select(cluster => cluster.Outcomes()))
            {
                foreach (var entry in dict)
                {
                    if (!outcomes.ContainsKey(entry.Key))
                    {
                        outcomes[entry.Key] = entry.Value;
                    }
                    else
                    {
                        outcomes[entry.Key] += entry.Value;
                    }
                }
            }

            return outcomes;
        }
    }
}