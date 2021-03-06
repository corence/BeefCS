﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs
{
    public class ScanNode
    {
        public readonly Task task;
        public double profit;
        public readonly Dictionary<Resource, double> positions;
        public readonly Dictionary<Resource, double> openNeeds;
        private readonly long id; // because c# doesn't appear to have a reference equality comparer -- wtf
        private static long nextId = 1;

        public ScanNode(Task task, double profit, Dictionary<Resource, double> positions)
        {
            this.task = task;
            this.profit = profit;
            this.positions = positions;
            this.openNeeds = task.needs.ToDictionary(pair => pair.Key, pair => pair.Value);
            id = nextId++;
        }

        public class ScanNodeComparer : Comparer<ScanNode>
        {
            public override int Compare(ScanNode x, ScanNode y)
            {
                int result1 = x.profit.CompareTo(y.profit);
                if (result1 != 0)
                {
                    return result1;
                }

                return x.id.CompareTo(y.id);
            }
        }

        public Dictionary<Resource, double> updatePositions(IReadOnlyDictionary<Resource, double> newPositions)
        {
            Dictionary<Resource, double> result = new Dictionary<Resource, double>(this.positions);

            foreach (var entry in newPositions)
            {
                // replace all positions that appear in newPositions; retain any that don't
                result[entry.Key] = entry.Value;
            }
            return result;
        }
    }
}
