using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class ScanNode
    {
        public readonly Task task;
        public readonly double cost;
        public readonly Dictionary<Pos, double> positions;
        public readonly List<Need> openNeeds;
        private readonly long id; // because c# doesn't appear to have a reference equality comparer -- wtf
        private static long nextId = 1;

        public ScanNode(Task task, double cost, Dictionary<Pos, double> positions)
        {
            this.task = task;
            this.cost = cost;
            this.positions = positions;
            this.openNeeds = new List<Need>(task.needs);
            id = nextId++;
        }

        public class ScanNodeComparer : Comparer<ScanNode>
        {
            public override int Compare(ScanNode x, ScanNode y)
            {
                int result1 = x.cost.CompareTo(y.cost);
                if (result1 != 0)
                {
                    return result1;
                }

                return x.id.CompareTo(y.id);
            }
        }

        public Dictionary<Pos, double> updatePositions(IReadOnlyDictionary<Pos, double> newPositions)
        {
            Dictionary<Pos, double> result = new Dictionary<Pos, double>(this.positions);

            foreach (var entry in newPositions)
            {
                // replace all positions that appear in newPositions; retain any that don't
                result[entry.Key] = entry.Value;
            }
            return result;
        }
    }
}
