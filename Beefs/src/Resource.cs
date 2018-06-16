using Beefs.util;
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

    public class Dimension
    {
        public readonly string name;

        public Dimension(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class Range
    {
        public readonly double min;
        public readonly double max;

        public Range(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Contains(double value)
        {
            return value >= min && value <= max;
        }

        public static Range Merge(Range a, Range b)
        {
            return new Range(Math.Min(a.min, b.min), Math.Max(a.max, b.max));
        }

        public bool Intersects(Range that)
        {
            return this.min < that.max && that.min < this.max;
        }
    }

    public class Zone : IVolume<Zone>
    {
        public readonly IReadOnlyDictionary<Dimension, Range> values;

        public Zone(IReadOnlyDictionary<Dimension, Range> values)
        {
            this.values = values;
        }

        public bool ContainsFully(IReadOnlyDictionary<Dimension, double> pos)
        {
            foreach (var entry in pos)
            {
                if (!values.ContainsKey(entry.Key))
                {
                    return false;
                }

                if (!values[entry.Key].Contains(entry.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public Zone Extend(Zone volume)
        {
            return new Zone(DictionaryMerge.Merge(
                this.values,
                volume.values,
                a => a,
                (a, b) => Range.Merge(a, b),
                b => b));
        }

        public bool Intersects(Zone that)
        {
            foreach (var entry in this.values)
            {
                if (that.values.ContainsKey(entry.Key))
                {
                    if (!entry.Value.Intersects(that.values[entry.Key]))
                    {
                        return false;
                    }
                }
            }

            foreach (var entry in that.values)
            {
                if (this.values.ContainsKey(entry.Key))
                {
                    if (!entry.Value.Intersects(this.values[entry.Key]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
