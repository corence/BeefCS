using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class Resource
    {
        public readonly string name;

        public Resource(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
