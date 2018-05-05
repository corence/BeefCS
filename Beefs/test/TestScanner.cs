using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSpec;
using FluentAssertions;

namespace Beefs.test
{
    class TestScanner : nspec
    {
        void tests()
        {
            it["tests"] = () => "hello".Should().Be("Omg");
        }
    }
}

