using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beefs;
using Shouldly;

namespace Beefs.games
{
    [TestFixture]
    public class TestSims
    {
        [Test]
        public void testDeep()
        {
            TheSims sims = new TheSims();

            List<Task> tasks = new List<Task>
            {
                sims.BakeFood(3, 0),
                sims.ServeDinner(9, 0),
                sims.BuyIngredients(-4, 0), // here's the fridge!
                sims.ChopIngredients(0, 0),
                sims.BakeFood(-4, 0) // a second oven exists
            };
            Dictionary<Need, double> desires = new Dictionary<Need, double>
            {
                { TheSims.cash, 8 }, // i hate spending cash 8 times as much as i hate walking
                { TheSims.social, 120 } // but OMG i love being social!
            };
            List<Repositioner> repositioners = new List<Repositioner>()
            {
                new PythagoreanRepositioner(1, new List<Need>() { TheSims.x, TheSims.z })
            };
            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Need, double> startingResources = new Dictionary<Need, double>
            {
                { TheSims.x, 0 },
                { TheSims.cash, 50 }
            };
            ScanNode result = new Scanner().Scan(context, startingResources);

            result.ShouldNotBeNull();
            result.task.name.ShouldBe("buy ingredients");
            double moveCost = (4 + 4 + 3 + 6) * 1.0; // the dude starts at x position 0 then moves from there
            double cashCost = 1 * 8.0; // buy ingredients costs 1 cash
            result.cost.ShouldBe(moveCost + cashCost);
        }

        [Test]
        public void testPickupCash()
        {
            // The Sim wants a TV -- that's worth massive Room
            // The Sim has a lot of cash, but hates spending it -- and doesn't have enough for a TV
            // The Sim could sell her daughter's painting (which she doesn't particularly like) to generate some cash (but not enough for a TV)
            // Conclusion: the Sim will sell the painting to get cash

            TheSims sims = new TheSims();

            List<Task> tasks = new List<Task>
            {
                sims.BuyFridgeAt(13, 14),
                sims.SellPaintingAt(13, 10)
            };

            Dictionary<Need, double> desires = new Dictionary<Need, double>
            {
                { TheSims.cash, 10 }, // each Simolean is worth 10 happiness to me
                { TheSims.fridge, 100 }, // woah i am seriously hankering for that fridge
                { TheSims.artwork, 7 } // hmm don't really care about that painting so much
            };

            List<Repositioner> repositioners = new List<Repositioner>
            {
                new PythagoreanRepositioner(1, new List<Need> { TheSims.x, TheSims.z })
            };

            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Need, double> startingResources = new Dictionary<Need, double>
            {
                { TheSims.x, 0 },
                { TheSims.z, 0 },
                { TheSims.cash, 50 }
            };
            ScanNode result = new Scanner().Scan(context, startingResources);

            result.ShouldNotBeNull();
            result.task.name.ShouldBe("buy ingredients");
            double moveCost = (4 + 4 + 3 + 6) * 1.0; // the dude starts at x position 0 then moves from there
            double cashCost = 1 * 8.0; // this is built in to "buy ingredients" task
            result.cost.ShouldBe(moveCost + cashCost);
        }
    }
}