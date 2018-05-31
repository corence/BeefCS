using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace Beefs.games.aero
{
    [TestFixture]
    public class TestAero
    {
        /*
        * Here is the grand list of scenarios that we need to check,
        * along with their planned implementations.
        * 
        * TASK OPTIMISER
        * 
        * Behaviours:
        *  - Over time, the desire to optimise a given task accumulates.
        *  - It accumulates when the task is considered; especially when the task is rejected.
        *  - Impossible, hotly-contested, inconvenient and expensive tasks are more likely to be optimised.
        *  - When a task is optimised, then the desire to optimise it resets.
        * 
        * Solution design:
        * When a task is completed, one of the task candidates (including the chosen task) will be eligible for optimisation.
        * To optimise, we randomly pick any one of the optimisation tasks that is available for the relevant Needs.
        * We then figure out -- if that task had been implemented, how much cheaper would this Task have been?
        * If it's not an improvement, it gets discarded. Or, if it fails to meet the trivialness threshold.
        * Else, we create an Optimise task, which defers to the optimisation task implementation, and records the profit.
        * When an Optimise task is Completed it's Deleted.
        * 
        */

        [Test]
        public void testOptimiserAccumulates()
        {
            Aero aero = new Aero();

            List<Task> tasks = new List<Task>
            {
                aero.ChopTree(9, 9),
                aero.CraftSpear(44, 44),
                aero.FortifyConstruction(7, 22, 22),
            };
            Dictionary<Resource, double> desires = new Dictionary<Resource, double>
            {
                { Aero.log, 1 },
                { Aero.spear, 5 },
                { Aero.hatchet, 3 },
            };
            List<Repositioner> repositioners = new List<Repositioner>()
            {
                new PythagoreanRepositioner(1, new List<Resource>() { Aero.x, Aero.z })
            };
            ScanContext context = new ScanContext(tasks, desires, repositioners);

            Dictionary<Resource, double> initialInventory = new Dictionary<Resource, double>
            {
                { Aero.hatchet, 1 },
            };

            Dictionary<Resource, double> initialPositions = new Dictionary<Resource, double>
            {
                { Aero.x, 0 },
                { Aero.z, 0 },
            };
            OptimizingContext ocontext = new OptimizingContext(context, initialInventory, initialPositions);

            OptimizingScanner scanner = new OptimizingScanner(new SpotScanner(context, initialInventory, initialPositions));

            scanner.Scan(ocontext);
            scanner.optimizationSolutions.Count.ShouldBe(1);

            scanner.Scan(ocontext);
            scanner.optimizationSolutions.Count.ShouldBe(2); // hmm really? not if the cluster gets augmented
        }

        /*
         * 1) Two drinking fountains next to each other. Nobody else is using them.
         *    After drinking 1000 times, the dude still doesn't want to create a new one.
         */

        /* 2) Two drinking fountains next to each other. Three dudes are repeatedly drinking.
         *    After n iterations, a third drinking fountain is constructed.
         */


        /* 3) On this map:
         * 
         *  GoldMine
         *  <- River ->                    Bridge
         *     TownHall
         *  <- River ->                    Bridge
         *                                GoldMine
         *                                
         * If 100 peasants are mining gold from both mines, they will build a bridge over the north river,
         * but probably never build one over the south.
         */

        /* 4) Declaring war
         * This is also an optimisation for various resources!
         */


        /* 5) Hiring mercenaries
         * This is a more complex optimise task because the mercenary needs to proffer what they offer.
         * The client might need any of the abilities of the merc.
         * The merc might also offer their equipment for sale? But probably not, because that'd be weird.
         */

        /* 6) Merging desires
         * If three distinct tasks, in distinct locations, can benefit from a north bridge...
         * and two can benefit from a south bridge...
         * then the North bridge always gets built first,
         * the the South bridge will follow up.
         */


        /* TASK CLUSTERER
         * 
         * Two tasks that are "close" in positions should be clustered.
         * A Task Cluster is a Task which stores a Task along with a Count.
         * Whenever it gets combined with another Task, we take the mean of the positions of each, weighted by count,
         * to merge them together.
         * Definition of "close" tasks: the difference between the two tasks' positions is less than their outcome profit.
         * (^ to be tested and iterated)
         * 
         * TASK CONTENTION
         * 
         * (I think we want to manage this, rather than leaving it up to the game!)
         * Each Task has a contention count -- how many dudes can concurrently work on it.
         * When a ScanSpot is selected by a dude, each Task in that ScanSpot should be added to a Reserved list with a count.
         * Tasks will be removed from that list when the game reports to the shepherd that the tasks were Completed or Abandoned.
         * While scanning, any Task will be discarded from consideration if its contention count is met or exceeded.
         * 
         */
    }
}
