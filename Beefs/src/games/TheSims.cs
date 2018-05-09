using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.games
{
    public class TheSims
    {
        public class Item : Resource
        {
            public Item(string name) : base(name)
            {
            }
        }

        public static readonly Resource social = new Resource("social");
        public static readonly Item food = new Item("food");
        public static readonly Item choppedIngredients = new Item("chopped ingredients");
        public static readonly Item rawIngredients = new Item("raw ingredients");
        public static readonly Item cash = new Item("cash");
        public static readonly Item fridge = new Item("fridge");
        public static readonly Item artwork = new Item("artwork");
        public static readonly Resource x = new Resource("x");
        public static readonly Resource z = new Resource("z");

        public Task ServeDinner(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { food, 4 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { TheSims.x, x },
                { TheSims.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { social, 4 }
            };
            return new Task("serve dinner", needs, positions, outcomes);
        }

        public Task BakeFood(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { choppedIngredients, 1 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { TheSims.x, x },
                { TheSims.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { food, 1 }
            };
            return new Task("bake food", needs, positions, outcomes);
        }

        public Task ChopIngredients(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>
            {
                { rawIngredients, 1 }
            };
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { TheSims.x, x },
                { TheSims.z, z }
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { choppedIngredients, 1 }
            };
            return new Task("chop ingredients", needs, positions, outcomes);
        }

        public Task SellPaintingAt(int v1, int v2)
        {
            throw new NotImplementedException();
        }

        public Task BuyFridgeAt(int v1, int v2)
        {
            throw new NotImplementedException();
        }

        public Task BuyIngredients(double x, double z)
        {
            Dictionary<Resource, double> needs = new Dictionary<Resource, double>();
            Dictionary<Resource, double> positions = new Dictionary<Resource, double>
            {
                { TheSims.x, x },
                { TheSims.z, z },
            };
            Dictionary<Resource, double> outcomes = new Dictionary<Resource, double>
            {
                { rawIngredients, 1 },
                { cash, -1 }
            };
            return new Task("buy ingredients", needs, positions, outcomes);
        }
    }
}
