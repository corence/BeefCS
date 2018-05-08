using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class TheSims
    {
        public class Item : Need
        {
        }

        public static readonly Need social = new Need();
        public static readonly Item food = new Item();
        public static readonly Item choppedIngredients = new Item();
        public static readonly Item rawIngredients = new Item();
        public static readonly Need x = new Need();
        public static readonly Need z = new Need();

        public Task ServeDinner(double x, double z)
        {
            Dictionary<Need, double> needs = new Dictionary<Need, double>
            {
                { food, 4 }
            };
            Dictionary<Need, double> positions = new Dictionary<Need, double>
            {
                { TheSims.x, x },
                { TheSims.z, z }
            };
            Dictionary<Need, double> outcomes = new Dictionary<Need, double>
            {
                { social, 4 }
            };
            return new Task("serve dinner", needs, positions, outcomes);
        }

        public Task BakeFood(double x, double z)
        {
            Dictionary<Need, double> needs = new Dictionary<Need, double>
            {
                { choppedIngredients, 1 }
            };
            Dictionary<Need, double> positions = new Dictionary<Need, double>
            {
                { TheSims.x, x },
                { TheSims.z, z }
            };
            Dictionary<Need, double> outcomes = new Dictionary<Need, double>
            {
                { food, 1 }
            };
            return new Task("bake food", needs, positions, outcomes);
        }

        public Task ChopIngredients(double x, double z)
        {
            Dictionary<Need, double> needs = new Dictionary<Need, double>
            {
                { rawIngredients, 1 }
            };
            Dictionary<Need, double> positions = new Dictionary<Need, double>
            {
                { TheSims.x, x },
                { TheSims.z, z }
            };
            Dictionary<Need, double> outcomes = new Dictionary<Need, double>
            {
                { choppedIngredients, 1 }
            };
            return new Task("chop ingredients", needs, positions, outcomes);
        }
    }
}
