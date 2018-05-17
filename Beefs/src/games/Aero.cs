using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.src.games
{
    // A made-up game kinda like war3
    class Aero
    {
        public readonly Resource log = new Resource("log");
        public readonly Resource tree = new Resource("tree");
        public readonly Resource hatchet = new Resource("hatchet");
        public readonly Resource constructionSite = new Resource("construction site");
        public readonly Resource delay = new Resource("delay");
        public readonly Resource x = new Resource("x");
        public readonly Resource z = new Resource("z");

        public Task ChopTree(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { hatchet, 1 },
                { tree, 1 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { this.x, x },
                { this.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { log, 1 },
                { tree, -1 },
                { delay, 7 }
            };
            return new Task("serve dinner", needs, positions, outcomes);
        }

        public Task FortifyConstruction(long itemId, double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { log, 1 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { this.x, x },
                { this.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { log, -1 },
            };
            return new Task("serve dinner", needs, positions, outcomes);
        }
    }
}
