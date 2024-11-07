using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
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

        /// <summary>
        /// Only Works for sizes that are power of 2
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator BitonicSort(List<T> list, float delay)
        {
            ResetCounts();
            yield return RecursiveBitonicSort(list, 0, list.Count, 1, delay);
        }

        private static IEnumerator RecursiveBitonicSort(List<T> list, int from, int count, int dir, float delay)
        {
            if (count <= 1)
                yield break;

            int half = count / 2;

            yield return RecursiveBitonicSort(list, from, half, 1, delay);
            yield return RecursiveBitonicSort(list, from + half, half, 0, delay);
            yield return BitonicMerge(list, from, count, dir, delay);
        }

        private static IEnumerator BitonicMerge(List<T> list, int from, int count, int dir, float delay)
        {
            if (count <= 1) yield break;

            int half = count / 2;

            for (int i = from; i < from + half; i++)
            {
                if ((Compare(list[i], list[i + half]) > 0 && dir == 1) || (Compare(list[i], list[i + half]) < 0 && dir == 0))
                {
                    Swap(list, i, i + half);
                    yield return new WaitForSeconds(delay);
                }
            }

            yield return BitonicMerge(list, from, half, dir, delay);
            yield return BitonicMerge(list, from + half, half, dir, delay);
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

        public static IEnumerator QuickSort(List<T> list, float delay)
        {
            ResetCounts();
            yield return RecursiveQuickSort(list, 0, list.Count - 1, delay);
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
            // int bytesSize = Marshal.SizeOf<T>();
            // byte[] bytes = new byte[bytesSize];
            // for (int i = 0; i < bytesSize; i++)
            // {
            //     bytes[i] = Marshal.ReadByte(intptr);
            // }
            // BitArray bits = new BitArray(bytes);
            // Debug.Log(bits.Length);
            throw new NotImplementedException();
        }

        public static IEnumerator RadixMSDSort(List<T> list, float delay)
        {
            throw new NotImplementedException();
        }

        public static IEnumerator ShellSort(List<T> list, float delay)
        {
            ResetCounts();
            for (int gap = list.Count / 2; gap > 0; gap /= 2)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T aux = list[i];
                    int j;
                    for (j = i; j >= gap && Compare(list[j - gap], aux) > 0; j -= gap)
                    {
                        Swap(list, j, j - gap);
                        yield return new WaitForSeconds(delay);
                    }

                    list[j] = aux;
                    UpdateIterationCount();
                    onListUpdated?.Invoke(list);
                }
            }
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
            ResetCounts();
            //Recursive Quantity of steps
            int depthLimit = (int)(2 * Math.Floor(Math.Log(list.Count) / Math.Log(2)));
            yield return RecursiveIntroSort(list, 0, list.Count - 1, depthLimit, delay);
        }

        private static IEnumerator RecursiveIntroSort(List<T> list, int from, int to, int depth, float delay)
        {
            int maxInsertionCount = 16;
            if (list.Count > maxInsertionCount)
            {
                if (depth == 0)
                {
                    yield return HeapSort(list, delay);
                    yield break;
                }

                depth -= 1;

                int pivot = FindPivot(list, from, from + (to - from), to);
                Swap(list, pivot, to);

                int divisionIndex = Partition(list, from, to);

                yield return RecursiveIntroSort(list, from, divisionIndex - 1, depth, delay);
                yield return RecursiveIntroSort(list, divisionIndex + 1, to, depth, delay);
            }
            else
            {
                yield return InsertionSort(list, delay);
            }
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
            ResetCounts();
            int gnome = 0;
            while (gnome < list.Count)
            {
                if (gnome == 0)
                    gnome++;
                else if (Compare(list[gnome - 1], list[gnome]) <= 0)
                    gnome++;
                else
                {
                    Swap(list, gnome, gnome - 1);
                    gnome--;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        public static IEnumerator MergeSort(List<T> list, float delay)
        {
            ResetCounts();
            yield return RecursiveMergeSort(list, 0, list.Count - 1, delay);
        }

        private static IEnumerator RecursiveMergeSort(List<T> list, int from, int to, float delay)
        {
            if (from >= to) yield break;

            int middle = from + (to - from) / 2;

            yield return RecursiveMergeSort(list, from, middle, delay);
            yield return RecursiveMergeSort(list, middle + 1, to, delay);
            yield return Merge(list, from, middle, to, delay);
        }

        public static IEnumerator HeapSort(List<T> list, float delay)
        {
            ResetCounts();
            yield return Heapify(list, list.Count - 1, delay);
            for (int i = list.Count - 1; i > 1; i--)
            {
                Swap(list, 0, i);
                yield return new WaitForSeconds(delay);
                yield return Heapify(list, i - 1, delay);
            }
        }

        private static IEnumerator Heapify(List<T> list, int lastIndex, float delay)
        {
            if (lastIndex == 1)
            {
                if (Compare(list[1], list[0]) < 0)
                    Swap(list, 0, 1);
                yield break;
            }

            for (int i = lastIndex; i > 1; i -= 2)
            {
                int biggestLeafIndex = i;
                int parentIndex = (i - 2) / 2;

                if (Compare(list[i], list[i - 1]) < 0)
                    biggestLeafIndex = i - 1;

                if (i % 2 != 0)
                {
                    parentIndex = (i - 1) / 2;
                    biggestLeafIndex = i;
                    i++;
                }

                if (Compare(list[biggestLeafIndex], list[parentIndex]) > 0)
                {
                    Swap(list, biggestLeafIndex, parentIndex);
                    yield return new WaitForSeconds(delay);
                }
            }
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
                if (Compare(list[i - 1], list[i]) > 0)
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

        private static IEnumerator Merge(List<T> list, int from, int middle, int to, float delay)
        {
            int leftLastPos = middle - from + 1;
            int rightLastPos = to - middle;

            List<T> leftHalf = new List<T>();
            List<T> rightHalf = new List<T>();
            int leftIndex, rightIndex;

            for (leftIndex = 0; leftIndex < leftLastPos; ++leftIndex)
                leftHalf.Add(list[from + leftIndex]);
            for (rightIndex = 0; rightIndex < rightLastPos; ++rightIndex)
                rightHalf.Add(list[middle + 1 + rightIndex]);

            leftIndex = 0;
            rightIndex = 0;

            int auxIndex = from;
            while (leftIndex < leftLastPos && rightIndex < rightLastPos)
            {
                if (Compare(leftHalf[leftIndex], rightHalf[rightIndex]) <= 0)
                {
                    list[auxIndex] = leftHalf[leftIndex];
                    leftIndex++;
                }
                else
                {
                    list[auxIndex] = rightHalf[rightIndex];
                    rightIndex++;
                }

                UpdateIterationCount();
                onListUpdated?.Invoke(list);
                yield return new WaitForSeconds(delay);
                auxIndex++;
            }

            while (leftIndex < leftLastPos)
            {
                list[auxIndex] = leftHalf[leftIndex];
                leftIndex++;
                auxIndex++;

                UpdateIterationCount();
                onListUpdated?.Invoke(list);
                yield return new WaitForSeconds(delay);
            }

            while (rightIndex < rightLastPos)
            {
                list[auxIndex] = rightHalf[rightIndex];
                rightIndex++;
                auxIndex++;

                UpdateIterationCount();
                onListUpdated?.Invoke(list);
                yield return new WaitForSeconds(delay);
            }
        }

        private static int FindPivot(List<T> list, int i, int j, int k)
        {
            //Index Out of range exception
            int max = i;
            if (Compare(list[i], list[j]) < 0)
                max = j;
            if (Compare(list[k], list[j]) > 0 && Compare(list[k], list[i]) > 0)
                max = k;

            int min = i;
            if (Compare(list[i], list[j]) > 0)
                min = j;
            if (Compare(list[k], list[j]) < 0 && Compare(list[k], list[i]) < 0)
                min = k;

            if ((min == j && max == k) || (max == j && min == k))
                return i;
            if ((min == i && max == k) || (max == i && min == k))
                return j;

            return k;
        }

        private static int Partition(List<T> list, int from, int to)
        {
            T pivot = list[to];

            int i = (from - 1);

            for (int j = from; j <= to - 1; j++)
            {
                if (Compare(list[j], pivot) <= 0)
                {
                    i++;
                    Swap(list, i, j);
                }
            }

            Swap(list, i + 1, to);
            return (i + 1);
        }

        #endregion
    }
}