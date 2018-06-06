using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class ScanContext
    {
        public readonly IReadOnlyList<Task> tasks;
        public readonly IReadOnlyDictionary<Resource, double> desires;
        public readonly IReadOnlyCollection<Repositioner> repositioners;
        public readonly IReadOnlyDictionary<Resource, double> initialInventory;
        public readonly IReadOnlyDictionary<Resource, double> initialPositions;

        public ScanContext(IReadOnlyList<Task> tasks, IReadOnlyDictionary<Resource, double> desires, IReadOnlyCollection<Repositioner> repositioners, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            this.tasks = tasks;
            this.desires = desires;
            this.repositioners = repositioners;
            this.initialInventory = initialInventory;
            this.initialPositions = initialPositions;
        }
    }
}
