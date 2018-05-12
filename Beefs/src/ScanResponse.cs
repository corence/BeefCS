using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    class ScanPath
    {
        public Task firstTask;
        public List<ScanNode> stack;
        public List<ScanNode> allNodes;
        double profit;
        public readonly Dictionary<Resource, double> positions;

        public ScanPath(ScanNode root)
        {
            stack.Add(root);
            allNodes.Add(root);
        }

        public bool IsComplete()
        {
            return stack.Count == 0;
        }

        public void Scan(ScanContext context, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            if (IsComplete())
            {
                throw new Exception("bad state");
            }

            while (stack.Count > 0)
            {
                ScanNode successor = stack.Last();
                if (successor.openNeeds.Count == 0)
                {
                    stack.RemoveAt(stack.Count - 1);
                    continue;
                }

                Resource need = successor.openNeeds.Keys.Last();
                foreach (var entry in successor.openNeeds)
                {
                    successor.openNeeds.Remove(entry.Key);
                    if (entry.Value <= 0)
                    {
                        continue;
                    }

                    List<ScanPath> options = new List<ScanPath>();
                    foreach (var task in context.tasks.Where(task => task.outcomes.ContainsKey(need)))
                    {
                        ScanNode candidate = new ScanNode(task, context.profitOfTask(task, successor) + successor.profit, successor.updatePositions(task.positions));
                        if (candidate.task.needs.Count == 0)
                        {
                            // finally, the candidate will also be considered with the cost to get back to the scanner's initial position
                            candidate.profit -= context.repositioningCost(initialPositions, candidate.positions);
                        }
                        nodes.Add(candidate);
                    }
                }
            }

            // The stack count just dropped to zero. We have a viable path. Our last step is to add the movement cost from the start location to the initial task.
            profit -= context.repositioningCost(initialPositions, positions);
        }
    }
}
