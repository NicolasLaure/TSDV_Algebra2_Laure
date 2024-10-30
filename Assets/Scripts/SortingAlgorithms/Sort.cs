using System;
using System.Collections.Generic;

namespace SortingAlgorithms
{
    public static class Sort
    {
        public static bool IsSorted<T>(List<T> list) where T : IComparable
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1].CompareTo(list[i]) > 0)
                    return false;
            }

            return true;
        }
    }
}