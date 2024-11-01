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
            ResetCounts();
            int minIndex;
            for (int i = 0; i < list.Count; i++)
            {
                minIndex = i;
                for (int j = i; j < list.Count; j++)
                {
                    if (Compare(list[j], list[minIndex]) < 0)
                        minIndex = j;
                }

                Swap(list, i, minIndex);
                yield return new WaitForSeconds(delay);
            }
        }

        public static IEnumerator DoubleSelectionSort(List<T> list, float delay)
        {
            ResetCounts();
            int minIndex;
            int maxIndex;

            for (int i = 0; i < list.Count / 2; i++)
            {
                minIndex = i;
                maxIndex = list.Count - 1 - i;
                for (int j = i; j < list.Count - i; j++)
                {
                    if (Compare(list[j], list[minIndex]) < 0)
                        minIndex = j;
                }

                for (int j = list.Count - 1 - i; j > i; j--)
                {
                    if (Compare(list[j], list[maxIndex]) > 0)
                        maxIndex = j;
                }

                Swap(list, i, minIndex);
                Swap(list, list.Count - 1 - i, maxIndex);
                yield return new WaitForSeconds(delay);
            }
        }

        public static IEnumerator CocktailShakerSort(List<T> list, float delay)
        {
            ResetCounts();
            for (int i = 0; i < list.Count / 2; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (Compare(list[j], list[j + 1]) > 0)
                    {
                        Swap(list, j, j + 1);
                        yield return new WaitForSeconds(delay);
                    }
                }

                for (int j = list.Count - i - 2; j > i + 1; j--)
                {
                    if (Compare(list[j], list[j - 1]) < 0)
                    {
                        Swap(list, j, j - 1);
                        yield return new WaitForSeconds(delay);
                    }
                }
            }
        }

        public static IEnumerator QuickSort(List<T> list, int from, int to, float delay)
        {
            ResetCounts();
            yield return RecursiveQuickSort(list, from, to, delay);
        }

        private static IEnumerator RecursiveQuickSort(List<T> list, int from, int to, float delay)
        {
            if (to < from)
                yield break;

            int j = from - 1;
            for (int i = from; i < to; i++)
            {
                if (Compare(list[i], list[to]) < 0)
                {
                    j++;
                    Swap(list, i, j);
                    yield return new WaitForSeconds(delay);
                }
            }

            Swap(list, j + 1, to);

            int pivot = j + 1;

            yield return RecursiveQuickSort(list, from, pivot - 1, delay);
            yield return RecursiveQuickSort(list, pivot + 1, to, delay);
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
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (Compare(list[j], list[j + 1]) > 0)
                    {
                        Swap(list, j, j + 1);
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
            ResetCounts();
            for (int i = 0; i < list.Count; ++i)
            {
                int j = i - 1;

                while (j >= 0 && Compare(list[i], list[j]) < 0)
                    j--;

                InsertAt(list, i, j + 1);
                yield return new WaitForSeconds(delay);
            }
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
            oniterationCountUpdated?.Invoke(0);
            onComparissonUpdated?.Invoke(0);
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
            UpdateIterationCount();
            onListUpdated?.Invoke(list);
        }

        private static void InsertAt(List<T> list, int from, int to)
        {
            T aux = list[from];
            list.RemoveAt(from);
            list.Insert(to, aux);
            UpdateIterationCount();
            onListUpdated?.Invoke(list);
        }

        private static int Compare(T a, T b)
        {
            UpdateComparissonCount();
            return a.CompareTo(b);
        }

        #endregion
    }
}