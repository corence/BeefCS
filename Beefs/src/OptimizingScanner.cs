using Beefs.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class OptimizingScanner
    {
        private readonly Chooser chooser;
        private readonly SpotScanner scanner;
        public readonly Dictionary<string, TaskCluster> optimizationSolutions = new Dictionary<string, TaskCluster>();

        public OptimizingScanner(Chooser chooser, SpotScanner scanner)
        {
            this.chooser = chooser;
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
                ScanSpot optimizationTarget = chooser.ChooseElement(scanner.allVisitedSpots);
                if (optimizationTarget.tasks.Count > 0)
                {
                    Task targetTask = optimizationTarget.tasks.Last();
                    if (targetTask.needs.Count > 0)
                    {
                        Resource targetResource = chooser.ChooseElement(targetTask.needs.Keys, targetTask.needs.Count);

                        if (context.optimizationStrategies.ContainsKey(targetResource))
                        {
                            List<OptimizationStrategy> strategies = context.optimizationStrategies[targetResource];
                            if (strategies.Count > 0)
                            {
                                OptimizationStrategy strategy = chooser.ChooseElement(strategies);
                                IReadOnlyDictionary<Resource, double> positions = SeekToPositions(optimizationTarget.tasks);
                                Task solution = strategy.Optimize(optimizationTarget.terminalDesire, targetTask, positions);
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
            TaskCluster taskCluster;
            if (optimizationSolutions.ContainsKey(optimization.name))
            {
                taskCluster = new TaskCluster(optimizationSolutions[optimization.name], optimization, chooser);
            }
            else
            {
                taskCluster = new TaskCluster(optimization);
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
    }
}
