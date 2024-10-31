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


        #region SortingMethods

        public static IEnumerator BitonicSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator SelectionSort(List<T> list, float delay)
        {
            int minIndex;
            for (int i = 0; i < list.Count; i++)
            {
                minIndex = i;
                for (int j = i; j < list.Count; j++)
                {
                    if (list[j].CompareTo(list[minIndex]) < 0)
                        minIndex = j;
                }

                Swap(list, i, minIndex);
                yield return new WaitForSeconds(delay);
            }
        }

        public static IEnumerator CocktailShakerSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator QuickSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator RadixLSDSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator RadixMSDSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator ShellSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
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

        public static IEnumerator IntroSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator AdaptiveMergeSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
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
                        Swap(list, j, j + 1);
                        UpdateIterationCount();
                        yield return new WaitForSeconds(delay);
                    }
                }
            }
        }

        public static IEnumerator GnomeSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator MergeSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator HeapSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator InsertionSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Utilities

        public static bool IsSorted(List<T> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1].CompareTo(list[i]) > 0)
                    return false;
            }

            return true;
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

        private static void Swap(List<T> list, int firstIndex, int secondIndex)
        {
            T aux = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = aux;
            onListUpdated?.Invoke(list);
        }
        
        #endregion
    }
}