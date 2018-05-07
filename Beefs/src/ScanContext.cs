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
        public readonly IReadOnlyDictionary<Need, double> prices;

        public ScanContext(IReadOnlyList<Task> tasks, IReadOnlyDictionary<Need, double> prices)
        {
            this.tasks = tasks;
            this.prices = prices;
        }
    }
}
