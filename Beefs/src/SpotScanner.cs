using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class SpotScanner
    {
        public readonly SortedSet<ScanSpot> spots;

        public SpotScanner(ScanContext context)
        {
            spots = new SortedSet<ScanSpot>(new ScanSpot.SpotComparer());
            foreach (ScanSpot spot in DesirableSpots(context))
            {
                spots.Add(spot);
            }
        }

        public static List<ScanSpot> DesirableSpots(ScanContext context)
        {
            List<ScanSpot> terminalSpots = new List<ScanSpot>();

            foreach (Resource need in context.desires.Keys)
            {
                foreach (Task task in context.tasks)
                {
                    if (task.outcomes.ContainsKey(need) && task.outcomes[need] > 0)
                    {
                        terminalSpots.Add(new ScanSpot(context, need, new List<Task> { task }));
                    }
                }
            }
            return terminalSpots;
        }

        public ScanSpot Scan(ScanContext context)
        {
            while (spots.Count > 0)
            {
                ScanSpot spot = spots.Last();
                spots.Remove(spot);

                List<ScanSpot> nextSpots = spot.Scan();
                if (nextSpots == null)
                {
                    return spot;
                }
                else
                {
                    foreach (var nextSpot in nextSpots)
                    {
                        spots.Add(nextSpot);
                    }
                }
            }

            return null;
        }
    }
}
