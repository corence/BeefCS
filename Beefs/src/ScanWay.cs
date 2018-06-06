using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Way
    {
        public readonly ScanContext context;
        public readonly List<Task> tasks;
        public IReadOnlyDictionary<Resource, double> initialInventory;
        public IReadOnlyDictionary<Resource, double> initialPositions;
        public readonly int id;
        public static int nextId = 1;

        private Way(ScanContext context, IReadOnlyList<Task> tasks, Task predecessor,
            IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            this.id = nextId++;
            this.context = context;
            this.initialInventory = initialInventory;
            this.initialPositions = initialPositions;

            this.tasks = new List<Task>();
            foreach (Task task in tasks)
            {
                this.tasks.Add(task);
            }
            this.tasks.Add(predecessor);
        }

        public Way(ScanContext context, Task terminalTask,
            IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
            : this(context, new List<Task>(), terminalTask, initialInventory, initialPositions)
        {
        }

        public double Profit()
        {
            IReadOnlyDictionary<Resource, double> positions = this.initialPositions;
            double profit = 0;

            for (int i = this.tasks.Count - 1; i >= 0; --i)
            {
                Task task = this.tasks[i];
                profit += task.OutcomeProfits(context);
                profit -= task.RepositioningCost(context, positions);
                positions = mergePositions(positions, task.positions);
            }

            return profit;
        }

        public class WayComparer : Comparer<Way>
        {
            public override int Compare(Way x, Way y)
            {
                int result1 = x.Profit().CompareTo(y.Profit());
                if (result1 != 0)
                {
                    return result1;
                }

                return x.id.CompareTo(y.id);
            }
        }

        public static Dictionary<Resource, double> mergePositions(IReadOnlyDictionary<Resource, double> oldPositions, IReadOnlyDictionary<Resource, double> newPositions)
        {
            Dictionary<Resource, double> result = new Dictionary<Resource, double>();
            
            foreach (var entry in oldPositions)
            {
                // retain any positions that won't appear in newPositions
                result[entry.Key] = entry.Value;
            }

            foreach (var entry in newPositions)
            {
                // replace all positions that appear in newPositions; retain any that don't
                result[entry.Key] = entry.Value;
            }

            return result;
        }

        public static Dictionary<Resource, double> applyOutcomes(IReadOnlyDictionary<Resource, double> oldInventory, IReadOnlyDictionary<Resource, double> outcomes)
        {
            Dictionary<Resource, double> result = new Dictionary<Resource, double>();

            foreach (var entry in oldInventory)
            {
                // retain existing inventory
                result[entry.Key] = entry.Value;
            }

            foreach (var entry in outcomes)
            {
                // mutate by the outcomes
                if (result.ContainsKey(entry.Key))
                {
                    result[entry.Key] += entry.Value;
                }
                else
                {
                    result[entry.Key] = entry.Value;
                }
            }

            return result;
        }

        public static IReadOnlyDictionary<Resource, double> CurrentInventory(IEnumerable<Task> tasks, IReadOnlyDictionary<Resource, double> initialInventory)
        {
            IReadOnlyDictionary<Resource, double> inventory = initialInventory;
            foreach (Task task in tasks)
            {
                inventory = applyOutcomes(inventory, task.outcomes);
            }
            return inventory;
        }

        public static Dictionary<Resource, double> CurrentPositions(IEnumerable<Task> tasks)
        {
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>();
            foreach (Task task in tasks)
            {
                positions = mergePositions(positions, task.positions);
            }
            return positions;
        }

        public static List<Task> FindSolutionTasks(IReadOnlyList<Task> tasks, Resource need)
        {
            List<Task> solutions = new List<Task>();
            foreach (Task contender in tasks)
            {
                if (contender.outcomes.ContainsKey(need) && contender.outcomes[need] > 0)
                {
                    solutions.Add(contender);
                }
            }
            return solutions;
        }

        public List<Way> Scan()
        {

            // check this out:
            // 1) initialPositions contains all positions of the dude, not just spatials
            // 2) positions matches it (initial is thrown away)
            // 3) now we pick all of the tasks that increase one of the desires by at least 1
            // 4) prioritize them based on their cost (which is 0 so far) and their outcome (which is their outcomes * desires)
            // 5) for the one you picked, can it be solved by your position?
            //   -> if so, then add it to the queue and update positions
            //   -> if not, then arbitrarily pick one of the unsatisfied and choose a task to resolve it
            // 6) if the current task is the terminal task, then that's it, we're done

            // update: NOPE
            // That sucks because we need to scan a whole lot before placing a single task. It doesn't work with the vast branching we're attempting.

            // New approach, similar but different.
            // 1) tasks is once again built from terminator to initiator.
            //  -> positions is built from empty, trawling back through the tasks.
            //  -> inventory is also built from empty.
            // 2) for each task that we encounter, 
            // 2) for the terminal, can positions be solved by positions, after traversing the task list? This means initialPos[cash] - pos[cash] will be charged at the going rate
            //   if so, then we're complete
            //   if not, then for *only* one of the unsatisified needs, make a new Way for each Task that can solve it
            // 4) there's no stack. Ultimately the "tasks" list will have a bunch of copies of the terminal task from each time it was reset

            IReadOnlyDictionary<Resource, double> positions = new Dictionary<Resource, double>();
            IReadOnlyDictionary<Resource, double> inventory = new Dictionary<Resource, double>();

            // move up to the current positions and inventory
            foreach (Task task in tasks)
            {
                positions = mergePositions(positions, task.positions);
                inventory = applyOutcomes(inventory, task.outcomes);
            }

            Resource need = null;

            foreach (var entry in tasks.Last().needs)
            {
                if (inventory.ContainsKey(entry.Key) && inventory[entry.Key] - entry.Value >= 0)
                {
                    // This need is already satisfied
                    continue;
                }
                else
                {
                    need = entry.Key;
                    break;
                }
            }

            if (need != null)
            {
                List<Way> ways = new List<Way>();

                // This need is unsatisfied; let's address it
                foreach (Task solution in context.tasks)
                {
                    if (solution.outcomes.ContainsKey(need) && solution.outcomes[need] > 0)
                    {
                        // this solution is a viable option
                        ways.Add(new Way(context, this.tasks, solution, initialInventory, initialPositions));
                    }
                }

                return ways;
            }
            else
            {
                // we are probably done but let's double-check
                foreach (var inv in inventory)
                {
                    if (inv.Value < 0)
                    {
                        throw new Exception("Illegal state here -- inv " + inv.Key + " was " + inv.Value);
                    }
                }
            }

            // we are done so i guess we return null
            return null;
        }
    }
}
