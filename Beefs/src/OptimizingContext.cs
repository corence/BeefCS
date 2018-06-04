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
        public readonly IReadOnlyDictionary<Resource, List<OptimizationStrategy>> optimizationStrategies;

        public OptimizingContext(IReadOnlyDictionary<Resource, List<OptimizationStrategy>> optimizationStrategies, ScanContext context)
        {
            this.optimizationStrategies = optimizationStrategies;
            this.scanContext = context;
        }
    }
}
