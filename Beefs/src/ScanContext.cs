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
        public readonly IReadOnlyDictionary<Resource, double> initialInventory;
        public readonly IReadOnlyDictionary<Resource, double> initialPositions;

        public ScanContext(IReadOnlyList<Task> tasks, IReadOnlyDictionary<Resource, double> desires, IReadOnlyCollection<Repositioner> repositioners, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            this.tasks = tasks;
            this.desires = desires;
            this.repositioners = repositioners;
            this.initialInventory = initialInventory;
            this.initialPositions = initialPositions;
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
                if (desires.ContainsKey(outcome.Key))
                {
                    profit += desires[outcome.Key] * outcome.Value;
                }
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
