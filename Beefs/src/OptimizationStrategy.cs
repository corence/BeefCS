using System.Collections.Generic;

namespace Beefs
{
    public interface OptimizationStrategy
    {
        Task Optimize(Resource terminalDesire, Task intent, IReadOnlyDictionary<Resource, double> positions);
    }
}