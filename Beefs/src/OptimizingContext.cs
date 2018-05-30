using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class OptimizingContext
    {
        public readonly ScanContext scanContext;
        public readonly IReadOnlyDictionary<Resource, double> initialInventory;
        public readonly IReadOnlyDictionary<Resource, double> initialPositions;
        public readonly IReadOnlyDictionary<Resource, List<OptimizationStrategy>> optimizationStrategies;

        public OptimizingContext(ScanContext context, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            this.scanContext = context;
            this.initialInventory = initialInventory;
            this.initialPositions = initialPositions;
        }
    }
}
