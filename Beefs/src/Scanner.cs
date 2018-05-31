using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Scanner
    {
        public Dictionary<Resource, double> Empty()
        {
            return new Dictionary<Resource, double>();
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

        public ScanSpot ScanForSpots(ScanContext context)
        {
            SortedSet<ScanSpot> spots = new SortedSet<ScanSpot>(new ScanSpot.SpotComparer());
            foreach (ScanSpot spot in SpotScanner.DesirableSpots(context))
            {
                spots.Add(spot);
            }

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

        public Way ScanForWays(ScanContext context, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            SortedSet<Way> ways = new SortedSet<Way>(new Way.WayComparer());
            foreach (Task task in DesirableTasks(context))
            {
                ways.Add(new Way(context, task, initialInventory, initialPositions));
            }

            while(ways.Count > 0)
            {
                Way way = ways.Last();
                ways.Remove(way);

                List<Way> nextWays = way.Scan();
                if (nextWays == null)
                {
                    return way;
                }
                else
                {
                    foreach (var nextWay in nextWays)
                    {
                        ways.Add(nextWay);
                    }
                }
            }

            return null;
        }

        public ScanPath ScanForPaths(ScanContext context, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            // Initialise our scan with a terminal node
            ScanNode terminator = new ScanNode(new Task("terminator", context.desires, Empty(), Empty()), 0, Empty());
            ScanPath terminatorPath = new ScanPath(terminator, initialPositions);
            SortedSet<ScanPath> paths = new SortedSet<ScanPath>(new ScanPath.ScanPathComparer());
            paths.Add(terminatorPath);

            while(paths.Count > 0)
            {
                ScanPath bestPath = paths.Last();
                paths.Remove(bestPath);

                List<ScanPath> extensions = bestPath.Scan(context);
                if (extensions == null)
                {
                    return bestPath;
                }
                else
                {
                    foreach (ScanPath p in extensions)
                    {
                        paths.Add(p);
                    }
                }
            }

            return null;
        }
    }
}
