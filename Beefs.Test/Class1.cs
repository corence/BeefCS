using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beefs;

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
            Assert.That(new ScanContext() != new ScanContext());
        }
    }
}
