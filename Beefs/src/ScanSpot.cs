using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class ScanSpot
    {
        public readonly int id;
        private readonly ScanContext context;
        public static int nextId = 1;
        public readonly IReadOnlyList<Task> tasks;
        private readonly IReadOnlyDictionary<Resource, double> initialInventory;
        private readonly IReadOnlyDictionary<Resource, double> initialPositions;

        public ScanSpot(ScanContext context, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions, IReadOnlyList<Task> tasks)
        {
            this.id = nextId++;
            this.context = context;
            this.initialInventory = initialInventory;
            this.initialPositions = initialPositions;
            this.tasks = tasks;
        }

        public class SpotComparer : Comparer<ScanSpot>
        {
            public override int Compare(ScanSpot x, ScanSpot y)
            {
                int result1 = x.Profit().CompareTo(y.Profit());
                if (result1 != 0)
                {
                    return result1;
                }

                return x.id.CompareTo(y.id);
            }
        }

        public double Profit()
        {
            IReadOnlyDictionary<Resource, double> positions = this.initialPositions;
            double profit = 0;

            for (int i = tasks.Count - 1; i >= 0; --i)
            {
                Task task = tasks[i];
                profit += context.outcomeProfits(task.outcomes);
                profit -= context.repositioningCost(task.positions, positions);
                positions = Way.mergePositions(positions, task.positions);
            }

            return profit;
        }

        public List<ScanSpot> Scan()
        {
            IReadOnlyDictionary<Resource, double> inventory = Way.CurrentInventory(tasks, initialInventory);
            IReadOnlyDictionary<Resource, double> positions = Way.CurrentPositions(tasks);

            Task successor = tasks.Last();
            foreach (var entry in successor.needs)
            {
                Resource need = entry.Key;
                double requiredQuantity = entry.Value;
                if (!inventory.ContainsKey(entry.Key) || inventory[entry.Key] < entry.Value)
                {
                    List<ScanSpot> options = new List<ScanSpot>();

                    // we need this resource and we don't yet have it
                    foreach (Task solution in Way.FindSolutionTasks(context.tasks, entry.Key))
                    {
                        List<Task> newTasks = new List<Task>(this.tasks);
                        newTasks.Add(solution);
                        options.Add(new ScanSpot(context, initialInventory, initialPositions, newTasks));
                    }

                    return options;
                }
            }

            // all needs are sated, and this is how we signal that
            return null;
        }
    }
}
