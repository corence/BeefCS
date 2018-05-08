using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class ScanContext
    {
        public readonly IReadOnlyList<Task> tasks;
        public readonly IReadOnlyDictionary<Need, double> prices;
        public readonly IReadOnlyCollection<Repositioner> repositioners;

        public ScanContext(IReadOnlyList<Task> tasks, IReadOnlyDictionary<Need, double> prices, IReadOnlyCollection<Repositioner> repositioners)
        {
            this.tasks = tasks;
            this.prices = prices;
            this.repositioners = repositioners;
        }

        public double costToMove(IReadOnlyDictionary<Pos, double> oldPositions, IReadOnlyDictionary<Pos, double> newPositions)
        {
            double cost = 0;

            foreach (var repositioner in repositioners)
            {
                cost += repositioner.costToMove(oldPositions, newPositions);
            }

            return cost;
        }
    }
}
