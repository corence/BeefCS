using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Task
    {
        public readonly string name;
        public readonly IReadOnlyCollection<Need> needs; // these must all be nonzero to complete this task
        public readonly Dictionary<Pos, double> positions; // to complete the task, all positions must be synced to these
        public readonly Dictionary<Need, double> outcomes;

        public Task(string name, IReadOnlyCollection<Need> needs, Dictionary<Pos, double> positions, Dictionary<Need, double> outcomes)
        {
            this.name = name;
            this.needs = needs;
            this.positions = positions;
            this.outcomes = outcomes;
        }
    }
}
