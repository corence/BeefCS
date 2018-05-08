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

        public double costOfTask(Task task, ScanNode successor)
        {
            return costToChangePositions(successor.positions, task.positions) + costToPurchaseProducts(task.charges);
        }

        public double costToPurchaseProducts(IReadOnlyDictionary<Need, double> charges)
        {
            double cost = 0;

            foreach (var charge in charges)
            {
                cost += prices[charge.Key] * charge.Value;
            }

            return cost;
        }

        public double costToChangePositions(IReadOnlyDictionary<Need, double> oldPositions, IReadOnlyDictionary<Need, double> newPositions)
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
