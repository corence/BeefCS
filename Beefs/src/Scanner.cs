using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Scanner
    {
        public ScanNode Scan(ScanContext context, Need terminalNeed)
        {
            // Initialise our scan with a terminal node
            List<Need> needs = new List<Need>();
            needs.Add(terminalNeed);
            ScanNode terminator = new ScanNode(new Task("terminator", needs, new Dictionary<Pos, double>(), new Dictionary<Need, double>()), 0, new Dictionary<Pos, double>());
            SortedSet<ScanNode> nodes = new SortedSet<ScanNode>(new ScanNode.ScanNodeComparer());
            nodes.Add(terminator);

            while(nodes.Count > 0)
            {
                ScanNode successor = nodes.First();
                nodes.Remove(successor);

                foreach (Need successorNeed in successor.task.needs)
                {
                    foreach(var task in context.tasks.Where(task => task.outcomes.ContainsKey(successorNeed)))
                    {
                        ScanNode candidate = new ScanNode(task, context.costToMove(successor.positions, task.positions) + successor.cost, successor.updatePositions(task.positions));
                        if (candidate.task.needs.Count == 0)
                        {
                            return candidate;
                        }
                        nodes.Add(candidate);
                    }
                }
            }

            return null;
        }
    }
}
