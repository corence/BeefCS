using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beefs;
using Shouldly;

namespace Beefs
{
    [TestFixture]
    public class TestSims
    {
        [Test]
        public void testDeep()
        {
            TheSims sims = new TheSims();

            List<Task> tasks = new List<Task>();
            tasks.Add(sims.BakeFood(3, 0));
            tasks.Add(sims.ServeDinner(9, 0));
            tasks.Add(sims.BuyIngredients(-4, 0)); // here's the fridge!
            tasks.Add(sims.ChopIngredients(0, 0));
            tasks.Add(sims.BakeFood(-4, 0)); // a second oven exists

            Dictionary<Need, double> prices = new Dictionary<Need, double>();
            prices.Add(TheSims.cash, 8); // i hate spending cash 8 times as much as i hate walking

            List<Repositioner> repositioners = new List<Repositioner>();
            repositioners.Add(new PythagoreanRepositioner(1, new List<Need>() { TheSims.x, TheSims.z }));

            ScanContext context = new ScanContext(tasks, prices, repositioners);

            ScanNode result = new Scanner().Scan(context, TheSims.social, new Dictionary<Need, double> { { TheSims.x, 0 } } );

            result.ShouldNotBeNull();
            result.task.name.ShouldBe("buy ingredients");
            double moveCost = (4 + 4 + 3 + 6) * 1.0;
            double cashCost = 1 * 8.0;
            result.cost.ShouldBe(moveCost + cashCost);
        }
    }
}
