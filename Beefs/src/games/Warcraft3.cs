using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.games
{
    class Warcraft3
    {
        public class Item : Resource
        {
            public Item(string name) : base(name)
            {
            }
        }

        public static readonly Resource baseStrength = new Resource("strength of base");
        public static readonly Resource delay = new Resource("delay");
        public static readonly Item wood = new Item("wood");
        public static readonly Item gold = new Item("gold");
        public static readonly Item food = new Item("food");
        public static readonly Resource x = new Resource("x");
        public static readonly Resource z = new Resource("z");

        public Task ChopTree(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>();
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { Warcraft3.x, x },
                { Warcraft3.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { wood, 1 },
                { delay, 7 } // it takes about 7 auts to chop down a tree in this world
            };
            return new Task("chop tree", needs, positions, outcomes);
        }

        public Task DesignateTower() // note that this task doesn't take an x/z; it's up to the game's tactical AI to decide where to place it (!)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>();
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>();
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { gold, -130 }, // these numbers are definitely made up
                { wood, -70 }, // hey what's the difference between a charge and a need? i'm not seeing it
                { baseStrength, 44 } // ha! this is tricksy, see. this Task says it will increase base strength, but in reality, *completing* the designated tower is what will increase base strength
            };
            return new Task("designate tower", needs, positions, outcomes);
        }
    }
}
