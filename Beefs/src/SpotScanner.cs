using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class SpotScanner
    {
        public readonly SortedSet<ScanSpot> spots;

        public SpotScanner(ScanContext context, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            spots = new SortedSet<ScanSpot>(new ScanSpot.SpotComparer());

            foreach (Task task in DesirableTasks(context))
            {
                spots.Add(new ScanSpot(context, initialInventory, initialPositions, new List<Task> { task }));
            }
        }

        private List<Task> DesirableTasks(ScanContext context)
        {
            List<Task> desirableTasks = new List<Task>();
            foreach (Resource need in context.desires.Keys)
            {
                foreach (Task task in context.tasks)
                {
                    if (task.outcomes.ContainsKey(need) && task.outcomes[need] > 0)
                    {
                        desirableTasks.Add(task);
                    }
                }
            }
            return desirableTasks;
        }

        public ScanSpot Scan(ScanContext context)
        {
            while (spots.Count > 0)
            {
                ScanSpot spot = spots.Last();
                spots.Remove(spot);

                List<ScanSpot> nextSpots = spot.Scan();
                if (nextSpots == null)
                {
                    return spot;
                }
                else
                {
                    foreach (var nextSpot in nextSpots)
                    {
                        spots.Add(nextSpot);
                    }
                }
            }

            return null;
        }
    }
}
