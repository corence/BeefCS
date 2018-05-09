using System;
using System.Collections.Generic;
using System.Linq;

namespace Beefs
{
    public interface Repositioner
    {
        double costToMove(IReadOnlyDictionary<Resource, double> oldPositions, IReadOnlyDictionary<Resource, double> newPositions);
    }

    public class SimpleRepositioner : Repositioner
    {
        public readonly double coefficient;
        public readonly Resource pos;

        public SimpleRepositioner(double coefficient, Resource pos)
        {
            this.coefficient = coefficient;
            this.pos = pos;
        }

        public double costToMove(IReadOnlyDictionary<Resource, double> oldPositions, IReadOnlyDictionary<Resource, double> newPositions)
        {
            if (oldPositions.ContainsKey(pos) && newPositions.ContainsKey(pos))
            {
                return coefficient * Math.Abs(newPositions[pos] - oldPositions[pos]);
            }
            else
            {
                return 0;
            }
        }
    }

    public class PythagoreanRepositioner : Repositioner
    {
        public readonly double coefficient;
        public readonly IReadOnlyCollection<Resource> poses;

        public PythagoreanRepositioner(double coefficient, IReadOnlyCollection<Resource> poses)
        {
            this.coefficient = coefficient;
            this.poses = poses;
        }

        public double costToMove(IReadOnlyDictionary<Resource, double> oldPositions, IReadOnlyDictionary<Resource, double> newPositions)
        {
            double squareDistance
                = poses
                .Where(pos => oldPositions.ContainsKey(pos))
                .Where(pos => newPositions.ContainsKey(pos)) // now we only have the poses that exist in both oldPosition and newPosition
                .Select(pos => newPositions[pos] - oldPositions[pos])
                .Select(diff => Math.Pow(diff, 2))
                .Sum();

            return Math.Sqrt(squareDistance); // return sqrt(x*x + y*y + z*z);
        }
    }
}