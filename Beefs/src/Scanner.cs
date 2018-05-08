using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Scanner
    {
        public Dictionary<Need, double> Empty()
        {
            return new Dictionary<Need, double>();
        }

        public ScanNode Scan(ScanContext context, Need terminalNeed, IReadOnlyDictionary<Need, double> initialPositions)
        {
            // Initialise our scan with a terminal node
            Dictionary<Need, double> needs = new Dictionary<Need, double>()
            { { terminalNeed, 1 } };
            ScanNode terminator = new ScanNode(new Task("terminator", needs, Empty(), Empty(), Empty()), 0, Empty());
            SortedSet<ScanNode> nodes = new SortedSet<ScanNode>(new ScanNode.ScanNodeComparer());
            nodes.Add(terminator);

            while(nodes.Count > 0)
            {
                ScanNode successor = nodes.First();
                nodes.Remove(successor);

                if (successor.task.needs.Count == 0)
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
                    Need successorNeed = needEntry.Key;
                    foreach(var task in context.tasks.Where(task => task.outcomes.ContainsKey(successorNeed)))
                    {
                        ScanNode candidate = new ScanNode(task, context.costOfTask(task, successor) + successor.cost, successor.updatePositions(task.positions));
                        if (candidate.task.needs.Count == 0)
                        {
                            // finally, the candidate will also be considered with the cost to get back to the scanner's initial position
                            candidate.cost += context.costToChangePositions(initialPositions, candidate.positions);
                        }
                        nodes.Add(candidate);
                    }
                }
            }

            return null;
        }
    }
}
