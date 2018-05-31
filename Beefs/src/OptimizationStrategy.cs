using System.Collections.Generic;

namespace Beefs
{
    public interface OptimizationStrategy
    {
        Task Optimize(Task intent, IReadOnlyDictionary<Resource, double> positions);
    }
}