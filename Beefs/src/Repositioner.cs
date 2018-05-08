using System;
using System.Collections.Generic;
using System.Linq;

namespace Beefs
{
    public interface Repositioner
    {
        double costToMove(IReadOnlyDictionary<Pos, double> oldPositions, IReadOnlyDictionary<Pos, double> newPositions);
    }

    public class SimpleRepositioner : Repositioner
    {
        public readonly double coefficient;
        public readonly Pos pos;

        public SimpleRepositioner(double coefficient, Pos pos)
        {
            this.coefficient = coefficient;
            this.pos = pos;
        }

        public double costToMove(IReadOnlyDictionary<Pos, double> oldPositions, IReadOnlyDictionary<Pos, double> newPositions)
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
        public readonly IReadOnlyCollection<Pos> poses;

        public PythagoreanRepositioner(double coefficient, IReadOnlyCollection<Pos> poses)
        {
            this.coefficient = coefficient;
            this.poses = poses;
        }

        public double costToMove(IReadOnlyDictionary<Pos, double> oldPositions, IReadOnlyDictionary<Pos, double> newPositions)
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