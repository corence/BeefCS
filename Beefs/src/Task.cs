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
        public readonly IReadOnlyDictionary<Resource, double> needs; // these must be met or exceeded to complete this task
        public readonly IReadOnlyDictionary<Resource, double> positions; // to complete the task, all positions must be synced to these
        public readonly IReadOnlyDictionary<Resource, double> outcomes; // these will be produced when the task is completed

        public Task(string name,
                    IReadOnlyDictionary<Resource, double> needs,
                    IReadOnlyDictionary<Resource, double> positions,
                    IReadOnlyDictionary<Resource, double> outcomes)
        {
            this.name = name;
            this.needs = needs;
            this.positions = positions;
            this.outcomes = outcomes;
        }
    }
}
