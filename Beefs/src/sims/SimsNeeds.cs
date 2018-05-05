using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.src.sims
{
    public class Item : Need { }

    public static class Sims
    {
        public static Pos x = new Pos();
        public static Pos y = new Pos();
        public static Pos z = new Pos();
        public static Need victory = new Need();
        public static Item food = new Item();
        public static Item choppedIngredients = new Item();
        public static Item rawIngredients = new Item();

        public static Task ServeDinner(double x, double z)
        {
            var needs = new List<Need>
            {
                food
            };

            var positions = new Dictionary<Pos, double>
            {
                [Sims.x] = x,
                [Sims.z] = z
            };

            var outcomes = new Dictionary<Need, double>
            {
                [victory] = 1
            };

            return new Task("serve dinner", needs, positions, outcomes);
        }

        public static Task Bake(double x, double z)
        {
            var needs = new List<Need>
            {
                choppedIngredients
            };

            var positions = new Dictionary<Pos, double>
            {
                [Sims.x] = x,
                [Sims.z] = z
            };

            var outcomes = new Dictionary<Need, double>
            {
                [food] = 1
            };

            return new Task("bake", needs, positions, outcomes);
        }

        public static Task ChopIngredients(double x, double z)
        {
            var needs = new List<Need>()
            {
                rawIngredients
            };

            var positions = new Dictionary<Pos, double>
            {
                [Sims.x] = x,
                [Sims.z] = z
            };

            var outcomes = new Dictionary<Need, double>
            {
                [choppedIngredients] = 1
            };

            return new Task("chop ingredients", needs, positions, outcomes);
        }

        public static Task BuyIngredients(double x, double z)
        {
            var needs = new List<Need>()
            {
            };

            var positions = new Dictionary<Pos, double>
            {
                [Sims.x] = x,
                [Sims.z] = z
            };

            var outcomes = new Dictionary<Need, double>
            {
                [choppedIngredients] = 1
            };

            return new Task("buy ingredients", needs, positions, outcomes);
        }
    }
}
