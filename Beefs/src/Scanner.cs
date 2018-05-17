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

        public ScanPath Scan(ScanContext context, IReadOnlyDictionary<Resource, double> initialPositions)
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
