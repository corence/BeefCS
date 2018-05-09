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
        public readonly IReadOnlyDictionary<Resource, double> desires;
        public readonly IReadOnlyCollection<Repositioner> repositioners;

        public ScanContext(IReadOnlyList<Task> tasks, IReadOnlyDictionary<Resource, double> desires, IReadOnlyCollection<Repositioner> repositioners)
        {
            this.tasks = tasks;
            this.desires = desires;
            this.repositioners = repositioners;
        }

        public double profitOfTask(Task task, ScanNode successor)
        {
            return outcomeProfits(task.outcomes) - repositioningCost(successor.positions, task.positions);
        }

        public double outcomeProfits(IReadOnlyDictionary<Resource, double> outcomes)
        {
            double profit = 0;

            foreach (var outcome in outcomes)
            {
                profit += desires[outcome.Key] * outcome.Value;
            }

            return profit;
        }

        public double repositioningCost(IReadOnlyDictionary<Resource, double> oldPositions, IReadOnlyDictionary<Resource, double> newPositions)
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
