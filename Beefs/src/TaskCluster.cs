using Beefs.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class TaskCluster
    {
        public readonly Task sample;
        public readonly IReadOnlyDictionary<Resource, double> positions;
        public int count;

        public TaskCluster(Task task)
        {
            sample = task;
            positions = task.positions;
            count = 1;
        }

        public TaskCluster(TaskCluster cluster, Task task, Chooser chooser)
        : this(cluster, new TaskCluster(task), chooser)
        {
        }

        public TaskCluster(TaskCluster cluster1, TaskCluster cluster2, Chooser chooser)
        {
            sample = chooser.Choose(cluster1.sample, cluster2.sample);
            positions = BlendPositions(cluster1, cluster2);
            count = cluster1.count + cluster2.count;
        }

        public static Task ChooseSample(TaskCluster cluster1, TaskCluster cluster2, Random random)
        {
            int index = random.Next(cluster1.count + cluster2.count);
            if (index < cluster1.count)
            {
                return cluster1.sample;
            }
            else
            {
                return cluster2.sample;
            }
        }

        public static IReadOnlyDictionary<Resource, double> BlendPositions(TaskCluster cluster1, TaskCluster cluster2)
        {
            Dictionary<Resource, double> result = new Dictionary<Resource, double>();

            foreach (var entry in cluster1.positions)
            {
                result.Add(entry.Key, entry.Value); // god damn it C#, why are your collections so obviously awful
            }

            foreach (var entry in cluster2.positions)
            {
                if (result.ContainsKey(entry.Key))
                {
                    // Merge them, weighted by counts
                    double merged = result[entry.Key] * cluster1.count + entry.Value * cluster2.count;
                    result.Add(entry.Key, merged / (cluster1.count + cluster2.count));
                }
                else
                {
                    result.Add(entry.Key, entry.Value);
                }
            }

            return result;
        }

        public IReadOnlyDictionary<Resource, double> Outcomes()
        {
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>();
            foreach (var entry in sample.outcomes)
            {
                outcomes[entry.Key] = entry.Value * count;
            }
            return outcomes;
        }
    }
}
