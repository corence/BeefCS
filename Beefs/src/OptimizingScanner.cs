using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class OptimizingScanner
    {
        private Random random = new Random();

        private readonly SpotScanner scanner;
        public readonly Dictionary<string, TaskCluster> optimizationSolutions = new Dictionary<string, TaskCluster>();

        public OptimizingScanner(SpotScanner scanner)
        {
            this.scanner = scanner;
        }

        public ScanSpot Scan(OptimizingContext context)
        {
            ScanSpot result = scanner.Scan(context.scanContext);
            TryOptimize(context, scanner.spots);
            return result;
        }

        private void TryOptimize(OptimizingContext context, IReadOnlyCollection<ScanSpot> spots)
        {
            if (spots.Count > 0)
            {
                ScanSpot optimizationTarget = ChooseRandomElement(scanner.spots);
                if (optimizationTarget.tasks.Count > 0)
                {
                    Task targetTask = optimizationTarget.tasks.Last();
                    if (targetTask.needs.Count > 0)
                    {
                        Resource targetResource = ChooseRandomElement(targetTask.needs.Keys, targetTask.needs.Count);

                        if (context.optimizationStrategies.ContainsKey(targetResource))
                        {
                            List<OptimizationStrategy> strategies = context.optimizationStrategies[targetResource];
                            if (strategies.Count > 0)
                            {
                                OptimizationStrategy strategy = ChooseRandomElement(strategies);
                                IReadOnlyDictionary<Resource, double> positions = SeekToPositions(optimizationTarget.tasks);
                                Task solution = strategy.Optimize(targetTask, positions);
                                if (solution != null)
                                {
                                    AddOptimizationSolution(solution);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddOptimizationSolution(Task optimization)
        {
            TaskCluster taskCluster = optimizationSolutions[optimization.name];
            if (taskCluster == null)
            {
                taskCluster = new TaskCluster(optimization);
            }
            else
            {
                taskCluster = new TaskCluster(taskCluster, optimization, random);
            }
            optimizationSolutions.Add(optimization.name, taskCluster);
        }

        public IReadOnlyDictionary<Resource, double> SeekToPositions(IReadOnlyList<Task> tasks)
        {
            IReadOnlyDictionary<Resource, double> positions = new Dictionary<Resource, double>();

            foreach (Task task in tasks)
            {
                positions = Way.mergePositions(positions, task.positions);
            }

            return positions;
        }

        public T ChooseRandomElement<T>(IReadOnlyCollection<T> collection)
        {
            return ChooseRandomElement(collection, collection.Count);
        }

        public T ChooseRandomElement<T>(IEnumerable<T> collection, int count)
        {
            int index = random.Next(count);
            int i = 0;
            foreach (T element in collection)
            {
                if (i == index)
                {
                    return element;
                }
                else
                {
                    ++i;
                }
            }

            throw new Exception("No element found...");
        }
    }
}
