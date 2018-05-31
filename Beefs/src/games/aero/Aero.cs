using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.games.aero
{
    // A made-up game kinda like war3
    public class Aero
    {
        public static readonly Resource log = new Resource("log");
        public static readonly Resource tree = new Resource("tree");
        public static readonly Resource spear = new Resource("spear");
        public static readonly Resource pickaxe = new Resource("pickaxe");
        public static readonly Resource hatchet = new Resource("hatchet");
        public static readonly Resource constructionSite = new Resource("construction site");
        public static readonly Resource delay = new Resource("delay");
        public static readonly Resource x = new Resource("x");
        public static readonly Resource z = new Resource("z");

        public Task CraftSpear(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { hatchet, 1 },
                { log, 3 } // ideally they should be dropping logs at the workshop rather than carrying them around. But how :thinking_face: Ahh, separate tasks -- the CreateSpear task wouldn't appear until 3 logs were dropped in
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { Aero.x, x },
                { Aero.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { log, -3 },
                { spear, 1 },
                { delay, 4 }
            };
            return new Task("craft spear", needs, positions, outcomes);
        }

        public Task CraftPickaxe(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { log, 3 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { Aero.x, x },
                { Aero.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { log, -3 },
                { pickaxe, 1 },
                { delay, 4 }
            };
            return new Task("craft spear", needs, positions, outcomes);
        }

        public static Task ConstructPickaxeFactory(IReadOnlyDictionary<Resource, double> positions)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { log, 3 }
                // ideally they should be dropping logs at the workshop rather than carrying them around. But how :thinking_face:
                // Ahh, separate tasks -- the CreatePickaxe task wouldn't appear until 3 logs were dropped in
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { log, -3 },
                { delay, 10 }
            };
            return new Task("construct pickaxe factory", needs, positions, outcomes);
        }

        public class OptimizePickaxeProduction : OptimizationStrategy
        {
            public Task Optimize(Resource terminalDesire, Task intent, IReadOnlyDictionary<Resource, double> positions)
            {
                return ConstructPickaxeFactory(positions);
            }
        }

        public Task ChopTree(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { hatchet, 1 },
                { tree, 1 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { Aero.x, x },
                { Aero.z, z }
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
                { Aero.x, x },
                { Aero.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { log, -1 },
            };
            return new Task("serve dinner", needs, positions, outcomes);
        }
    }
}
