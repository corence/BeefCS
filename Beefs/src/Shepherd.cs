using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// This is an additional layer that wraps scanning.
// It is responsible for the following:
// - when tasks are not being picked, but they are important, associated Rate tasks should gradually increase in desire
// - when tasks are completed, they should drop in desire
namespace Beefs
{
    class Shepherd
    {
        public readonly Scanner scanner;

        public Shepherd()
        {
            this.scanner = new Scanner();
        }

        public Task Scan(ScanContext context, IReadOnlyDictionary<Resource, double> initialInventory, IReadOnlyDictionary<Resource, double> initialPositions)
        {
            ScanSpot spot = scanner.ScanForSpots(context, initialInventory, initialPositions);
            return null;
        }
    }
}
