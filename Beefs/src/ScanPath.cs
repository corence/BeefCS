using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class ScanPath
    {
        public Task firstTask;
        public List<ScanNode> stack;
        public List<ScanNode> allNodes;
        public double profit;
        public readonly Dictionary<Resource, double> positions;
        public readonly IReadOnlyDictionary<Resource, double> initialPositions;
        public readonly int id;
        public static int nextId = 1;

        public ScanPath(ScanNode root, IReadOnlyDictionary<Resource, double> initialPositions)
            : this(null, new List<ScanNode>(), new List<ScanNode>(), 0, new Dictionary<Resource, double>(), initialPositions, root)
        {
        }

        public ScanPath(Task firstTask, List<ScanNode> stack, List<ScanNode> allNodes, double profit, Dictionary<Resource, double> positions, IReadOnlyDictionary<Resource, double> initialPositions, ScanNode candidate)
        {
            this.firstTask = firstTask;
            this.stack = new List<ScanNode>(stack);
            this.allNodes = new List<ScanNode>(allNodes);
            this.profit = profit;
            this.positions = new Dictionary<Resource, double>(positions);
            this.initialPositions = initialPositions;
            this.stack.Add(candidate);
            this.allNodes.Add(candidate);
            id = nextId++;
        }

        public class ScanPathComparer : Comparer<ScanPath>
        {
            public override int Compare(ScanPath x, ScanPath y)
            {
                int result1 = x.profit.CompareTo(y.profit);
                if (result1 != 0)
                {
                    return result1;
                }

                return x.id.CompareTo(y.id);
            }
        }

        public List<ScanPath> Scan(ScanContext context)
        {
            // Now, we loop until:
            // - we find a set of new paths
            // - we find no paths
            // - or we complete this path (which means stack.Count == 0)
            while (stack.Count > 0)
            {
                // If a ScanNode has no open needs then it is complete
                ScanNode successor = stack.Last();
                if (successor.openNeeds.Count == 0)
                {
                    this.profit += successor.profit;
                    this.profit -= context.repositioningCost(positions, successor.positions);
                    if (firstTask == null)
                    {
                        firstTask = successor.task;
                        this.profit -= context.repositioningCost(initialPositions, positions);
                    }
                    stack.RemoveAt(stack.Count - 1);
                    continue;
                }

                // So the successor has open needs. Let's check one of the open needs for new paths or for failures
                Resource need = successor.openNeeds.Keys.Last();
                double strength = successor.openNeeds[need];
                successor.openNeeds.Remove(need);
                if (strength <= 0)
                {
                    // This need is already satisfied so let's try the next one
                    continue;
                }

                // Check all tasks that might satisfy this open need, and wrap them as ScanPaths that extend this one
                List<ScanPath> solutions = new List<ScanPath>();
                foreach (var task in context.tasks.Where(task => task.outcomes.ContainsKey(need)))
                {
                    ScanNode candidate = new ScanNode(task, context.profitOfTask(task, successor), successor.updatePositions(task.positions));
                    ScanPath path = new ScanPath(firstTask, stack, allNodes, profit, positions, initialPositions, candidate);
                    solutions.Add(path);
                }

                // If solutions is empty then that's the end of the road for this path
                // and all paths that it derived from....
                // ...or is it?
                return solutions;
            }

            // We did it -- this path is complete.
            return null;
        }
    }
}
