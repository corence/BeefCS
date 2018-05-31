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

            Dictionary<Resource, double> initialInventory = new Dictionary<Resource, double>
            {
                { TheSims.cash, 50 } // sufficient cash
            };

            Dictionary<Resource, double> initialPositions = new Dictionary<Resource, double>
            {
                { TheSims.x, 1000 }
            };

            ScanContext context = new ScanContext(tasks, desires, repositioners, initialInventory, initialPositions);
            ScanSpot result = new Scanner().ScanForSpots(context);

            result.ShouldNotBeNull();
            result.tasks.Count.ShouldBe(2);
            result.tasks.Last().name.ShouldBe("buy ingredients");
            double moveCost = (1000 + 100 + 100 + 1000) * 1.0; // the dude starts at x position 1000 then moves to a negative space first
            double cashCost = 1 * 7.0; // buy ingredients costs 1 cash
            double reward = 1 * 13.0; // reward for chopped ingredients -- it's bad, but it's better to do something than nothing
            result.Profit().ShouldBe(reward - moveCost - cashCost);
        }

        [Test]
        public void testOneLevel()
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

            Dictionary<Resource, double> initialInventory = new Dictionary<Resource, double>
            {
                { TheSims.cash, 50 } // sufficient cash
            };

            Dictionary<Resource, double> initialPositions = new Dictionary<Resource, double>
            {
                { TheSims.x, 1000 }
            };

            ScanContext context = new ScanContext(tasks, desires, repositioners, initialInventory, initialPositions);
            ScanSpot result = new Scanner().ScanForSpots(context);

            result.ShouldNotBeNull();
            result.tasks.Count.ShouldBe(1);
            result.tasks.Last().name.ShouldBe("buy ingredients");
            double moveCost = (1000 + 100) * 1.0; // the dude starts at x position 1000 then moves to a negative space first
            double cashCost = 1 * 7.0; // buy ingredients costs 1 cash
            double reward = 1 * 9001.0; // reward for chopped ingredients -- it's bad, but it's better to do something than nothing
            result.Profit().ShouldBe(reward - moveCost - cashCost);
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

            Dictionary<Resource, double> initialInventory = new Dictionary<Resource, double>
            {
                { TheSims.cash, 50 } // sufficient cash
            };

            Dictionary<Resource, double> initialPositions = new Dictionary<Resource, double>
            {
                { TheSims.x, 0 }
            };

            ScanContext context = new ScanContext(tasks, desires, repositioners, initialInventory, initialPositions);
            ScanSpot result = new Scanner().ScanForSpots(context);

            result.ShouldNotBeNull();
            result.tasks.Count.ShouldBe(4);
            result.tasks.Last().name.ShouldBe("buy ingredients");
            double moveCost = (4 + 4 + 3 + 6) * 1.0; // the dude starts at x position 0 then moves from there
            double cashCost = 1 * 8.0; // buy ingredients costs 1 cash
            double socialReward = 4 * 120.0; // huge social reward for this sim
            result.Profit().ShouldBe(socialReward - moveCost - cashCost);
        }
    }
}