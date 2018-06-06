using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Task
    {
        public readonly string name;
        public readonly IReadOnlyDictionary<Resource, double> needs; // these must be met or exceeded to complete this task
        public readonly IReadOnlyDictionary<Resource, double> positions; // to complete the task, all positions must be synced to these
        public readonly IReadOnlyDictionary<Resource, double> outcomes; // these will be produced when the task is completed

        public Task(string name,
                    IReadOnlyDictionary<Resource, double> needs,
                    IReadOnlyDictionary<Resource, double> positions,
                    IReadOnlyDictionary<Resource, double> outcomes)
        {
            this.name = name;
            this.needs = needs;
            this.positions = positions;
            this.outcomes = outcomes;
        }

        public double OutcomeProfits(ScanContext context)
        {
            double profit = 0;

            foreach (var outcome in outcomes)
            {
                if (context.desires.ContainsKey(outcome.Key))
                {
                    profit += context.desires[outcome.Key] * outcome.Value;
                }
            }

            return profit;
        }

        public double RepositioningCost(ScanContext context, IReadOnlyDictionary<Resource, double> nextPositions)
        {
            double cost = 0;

            foreach (var repositioner in context.repositioners)
            {
                cost += repositioner.costToMove(positions, nextPositions);
            }

            return cost;
        }

        public double Profit(ScanContext context, IReadOnlyDictionary<Resource, double> nextPositions)
        {
            return OutcomeProfits(context) - RepositioningCost(context, nextPositions);
        }
    }
}
