using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.src.games
{
    class Warcraft3
    {
        public class Item : Need
        {
        }

        public static readonly Need baseStrength = new Need();
        public static readonly Need delay = new Need();
        public static readonly Item wood = new Item();
        public static readonly Item gold = new Item();
        public static readonly Item food = new Item();
        public static readonly Need x = new Need();
        public static readonly Need z = new Need();

        public Task ChopTree(double x, double z)
        {
            Dictionary<Need, double> needs = new Dictionary<Need, double>();
            Dictionary<Need, double> charges = new Dictionary<Need, double>()
            {
                { delay, 7 } // it takes about 7 auts to chop down a tree in this world
            };
            Dictionary<Need, double> positions = new Dictionary<Need, double>
            {
                { Warcraft3.x, x },
                { Warcraft3.z, z }
            };
            Dictionary<Need, double> outcomes = new Dictionary<Need, double>
            {
                { wood, 1 }
            };
            return new Task("chop tree", needs, charges, positions, outcomes);
        }

        public Task DesignateTower() // note that this task doesn't take an x/z; it's up to the game's tactical AI to decide where to place it (!)
        {
            Dictionary<Need, double> needs = new Dictionary<Need, double>();
            Dictionary<Need, double> charges = new Dictionary<Need, double>()
            {
                { gold, 130 }, // these numbers are definitely made up
                { wood, 70 } // hey what's the difference between a charge and a need? i'm not seeing it
            };
            Dictionary<Need, double> positions = new Dictionary<Need, double>();
            Dictionary<Need, double> outcomes = new Dictionary<Need, double>
            {
                { baseStrength, 44 } // ha! this is tricksy, see. this Task says it will increase base strength, but in reality, *completing* the designated tower is what will increase base strength
            };
            return new Task("designate tower", needs, charges, positions, outcomes);
        }
    }
}
