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
        public void testTwoLevels()
        {
            TheSims sims = new TheSims();

            List<Task> tasks = new List<Task>
            {
                sims.BakeFood(100, 0),
                sims.ServeDinner(100000, 0),
                sims.BuyIngredients(-100, 0), // here's the fridge!
                sims.ChopIngredients(1000, 0),
                sims.BakeFood(10000, 0) // a second oven exists
            };
            Dictionary<Resource, double> desires = new Dictionary<Resource, double>
            {
                { TheSims.cash, 7 }, // i hate spending cash 7 times as much as i hate walking
                { TheSims.social, 0 }, // don't even care about social,
                { TheSims.choppedIngredients, 13 } // chopped ingredients are pretty cool (they are the goal of this exercise)
            };
            List<Repositioner> repositioners = new List<Repositioner>()
            {
                new PythagoreanRepositioner(1, new List<Resource>() { TheSims.x, TheSims.z })
            };
            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Resource, double> startingResources = new Dictionary<Resource, double>
            {
                { TheSims.x, 1000 },
                { TheSims.cash, 50 } // sufficient cash
            };
            ScanPath result = new Scanner().Scan(context, startingResources);

            result.ShouldNotBeNull();
            result.firstTask.ShouldNotBeNull();
            result.firstTask.name.ShouldBe("buy ingredients");
            double moveCost = (1000 + 100 + 100 + 1000) * 1.0; // the dude starts at x position 1000 then moves to a negative space first
            double cashCost = 1 * 7.0; // buy ingredients costs 1 cash
            double reward = 1 * 13.0; // reward for chopped ingredients -- it's bad, but it's better to do something than nothing
            result.profit.ShouldBe(reward - moveCost - cashCost);
        }

        [Test]
        public void testZeroLevels()
        {
            TheSims sims = new TheSims();

            List<Task> tasks = new List<Task>
            {
                sims.BakeFood(100, 0),
                sims.ServeDinner(100000, 0),
                sims.BuyIngredients(-100, 0), // here's the fridge!
                sims.ChopIngredients(1000, 0),
                sims.BakeFood(10000, 0) // a second oven exists
            };
            Dictionary<Resource, double> desires = new Dictionary<Resource, double>
            {
                { TheSims.cash, 7 }, // i hate spending cash 7 times as much as i hate walking
                { TheSims.social, 0 }, // don't even care about social,
                { TheSims.choppedIngredients, 13 }, // chopped ingredients are pretty cool,
                { TheSims.rawIngredients, 9001 } // but OMG raw ingredients are the BUSINESS and are the goal here
            };
            List<Repositioner> repositioners = new List<Repositioner>()
            {
                new PythagoreanRepositioner(1, new List<Resource>() { TheSims.x, TheSims.z })
            };
            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Resource, double> startingResources = new Dictionary<Resource, double>
            {
                { TheSims.x, 1000 },
                { TheSims.cash, 50 } // sufficient cash
            };
            ScanPath result = new Scanner().Scan(context, startingResources);

            result.ShouldNotBeNull();
            result.firstTask.ShouldNotBeNull();
            result.firstTask.name.ShouldBe("buy ingredients");
            double moveCost = (1000 + 100) * 1.0; // the dude starts at x position 1000 then moves to a negative space first
            double cashCost = 1 * 7.0; // buy ingredients costs 1 cash
            double reward = 1 * 9001.0; // reward for chopped ingredients -- it's bad, but it's better to do something than nothing
            result.profit.ShouldBe(reward - moveCost - cashCost);
        }

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
            Dictionary<Resource, double> desires = new Dictionary<Resource, double>
            {
                { TheSims.cash, 8 }, // i hate spending cash 8 times as much as i hate walking
                { TheSims.social, 120 } // but OMG i love being social!
            };
            List<Repositioner> repositioners = new List<Repositioner>()
            {
                new PythagoreanRepositioner(1, new List<Resource>() { TheSims.x, TheSims.z })
            };
            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Resource, double> startingResources = new Dictionary<Resource, double>
            {
                { TheSims.x, 0 },
                { TheSims.cash, 50 }
            };
            ScanPath result = new Scanner().Scan(context, startingResources);

            result.ShouldNotBeNull();
            result.firstTask.ShouldNotBeNull();
            result.firstTask.name.ShouldBe("buy ingredients");
            double moveCost = (4 + 4 + 3 + 6) * 1.0; // the dude starts at x position 0 then moves from there
            double cashCost = 1 * 8.0; // buy ingredients costs 1 cash
            double socialReward = 4 * 120.0; // huge social reward for this sim
            result.profit.ShouldBe(socialReward - moveCost - cashCost);
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
                sims.SellArtworkAt(13, 10)
            };

            Dictionary<Resource, double> desires = new Dictionary<Resource, double>
            {
                { TheSims.cash, 10 }, // each Simolean is worth 10 happiness to me
                { TheSims.fridge, 100 }, // woah i am seriously hankering for that fridge
                { TheSims.artwork, 7 } // hmm don't really care about that painting so much
            };

            List<Repositioner> repositioners = new List<Repositioner>
            {
                new PythagoreanRepositioner(1, new List<Resource> { TheSims.x, TheSims.z })
            };

            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Resource, double> startingResources = new Dictionary<Resource, double>
            {
                { TheSims.x, 13 },
                { TheSims.cash, 50 }
            };
            ScanPath result = new Scanner().Scan(context, startingResources);

            result.ShouldNotBeNull();
            result.firstTask.name.ShouldBe("sell artwork");
            double moveCost    = 0 * 1.0; // since we never actually buy the fridge
            double cashWin     = 4 * 10.0; // this is built in to "buy ingredients" task
            double artworkCost = 1 * 7.0; // so long, picture
            result.profit.ShouldBe(cashWin - artworkCost - moveCost);
        }
    }
}