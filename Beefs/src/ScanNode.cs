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
        private readonly long id; // because c# doesn't appear to have a reference equality comparer -- wtf
        private static long nextId = 1;

        public ScanNode(Task task, double cost, Dictionary<Pos, double> positions)
        {
            this.task = task;
            this.cost = cost;
            this.positions = positions;
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

        public double costOf(Task task)
        {
            double newCost = 0;

            foreach (var entry in task.positions.Intersect(this.positions))
            {
                newCost += Math.Abs(task.positions[entry.Key] - this.positions[entry.Key]);
            }

            return newCost;
        }

        public Dictionary<Pos, double> updatePositions(Task task)
        {
            Dictionary<Pos, double> result = new Dictionary<Pos, double>();
            foreach (var entry in this.positions)
            {
                result[entry.Key] = entry.Value;
            }

            foreach (var entry in task.positions)
            {
                result[entry.Key] = entry.Value;
            }
            return result;
        }
    }
}
