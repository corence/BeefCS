using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class OptimizingScanner
    {
        private readonly Scanner scanner;
        public readonly Dictionary<string, TaskCluster> optimizationPlans = new Dictionary<string, TaskCluster>();

        public OptimizingScanner(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public ScanSpot Scan(ScanContext context, Dictionary<Resource, double> initialInventory, Dictionary<Resource, double> initialPositions)
        {
            return scanner.ScanForSpots(context, initialInventory, initialPositions);
        }
    }
}
