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

        public ScanNode Scan(ScanContext context, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            // Initialise our scan with a terminal node
            ScanNode terminator = new ScanNode(new Task("terminator", context.desires, Empty(), Empty()), 0, Empty());
            SortedSet<ScanNode> nodes = new SortedSet<ScanNode>(new ScanNode.ScanNodeComparer());
            nodes.Add(terminator);

            while(nodes.Count > 0)
            {
                ScanNode successor = nodes.Last();
                nodes.Remove(successor);

                if (successor.openNeeds.Count == 0)
                {
                    // This is it -- we found a worthy task
                    return successor;
                }

                foreach (var needEntry in successor.task.needs)
                {
                    if (needEntry.Value <= 0)
                    {
                        continue;
                    }
                    Resource successorNeed = needEntry.Key;
                    foreach(var task in context.tasks.Where(task => task.outcomes.ContainsKey(successorNeed)))
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

            return null;
        }
    }
}
