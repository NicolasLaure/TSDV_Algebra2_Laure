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
        public static event Action<int> oniterationCountUpdated;
        public static event Action<int> onComparissonUpdated;


        private static int iterationCount = 0;
        private static int comparissonCount = 0;

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
            ResetCounts();
            while (!IsSorted(list))
            {
                Shuffle(list);
                UpdateIterationCount();
                yield return new WaitForSeconds(delay);
            }
        }

        public static IEnumerator BubbleSort(List<T> list, float delay)
        {
            ResetCounts();
            T aux;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    UpdateComparissonCount();
                    if (list[j].CompareTo(list[j + 1]) > 0)
                    {
                        aux = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = aux;

                        onListUpdated?.Invoke(list);
                        UpdateIterationCount();
                        yield return new WaitForSeconds(delay);
                    }
                }
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

        private static void ResetCounts()
        {
            iterationCount = 0;
            comparissonCount = 0;
        }

        private static void UpdateIterationCount()
        {
            iterationCount++;
            oniterationCountUpdated?.Invoke(iterationCount);
        }

        private static void UpdateComparissonCount()
        {
            comparissonCount++;
            onComparissonUpdated?.Invoke(comparissonCount);
        }
    }
}