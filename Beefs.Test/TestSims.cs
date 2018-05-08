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
        public void testMath()
        {
            Assert.That(1 + 2 == 44);
        }

        [Test]
        public void testReference()
        {
            Assert.That(new Need() != new Need());
            Need n = new Need();
            Need q = n;
            Assert.That(q == n);
        }

        [Test]
        public void testDeep()
        {
            TheSims sims = new TheSims();

            List<Task> tasks = new List<Task>();
            tasks.Add(sims.BakeFood(3, 3));
            tasks.Add(sims.ServeDinner(9, 0));
            tasks.Add(sims.ChopIngredients(0, 0));
            tasks.Add(sims.BakeFood(-4, -4)); // a second oven exists

            Dictionary<Need, double> prices = new Dictionary<Need, double>();

            List<Repositioner> repositioners = new List<Repositioner>();
            repositioners.Add(new PythagoreanRepositioner(1, new List<Pos>() { TheSims.x, TheSims.z }));

            ScanContext context = new ScanContext(tasks, prices, repositioners);

            ScanNode result = new Scanner().Scan(context, TheSims.social);

            result.ShouldNotBeNull();
            result.task.name.ShouldBe("abc");
            result.cost.ShouldBe(5);
        }
    }
}
