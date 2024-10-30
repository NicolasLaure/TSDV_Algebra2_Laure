using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SortingAlgorithms
{
    public static class Sort<T> where T : IComparable<T>
    {
        public static event Action<List<T>> onListUpdated;
        public static event Action<int> onCountUpdated;


        private static int iterationCount = 0;

        public static bool IsSorted(List<T> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1].CompareTo(list[i]) > 0)
                    return false;
            }

            return true;
        }

        public static IEnumerator BogoSort(List<T> list, float delay)
        {
            iterationCount = 0;
            while (!IsSorted(list))
            {
                Shuffle(list);
                iterationCount++;
                onCountUpdated?.Invoke(iterationCount);
                yield return new WaitForSeconds(delay);
            }
        }

        public static void Shuffle(List<T> list)
        {
            T aux;
            int randomIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                randomIndex = Random.Range(0, list.Count);
                aux = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = aux;
            }

            onListUpdated?.Invoke(list);
        }
    }
}