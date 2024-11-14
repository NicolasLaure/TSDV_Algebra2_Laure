using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SortingAlgorithms
{
    public static class Sort<T> where T : IComparable<T>
    {
        public static event Action<List<T>> onListUpdated;
        public static event Action<int> oniterationCountUpdated;
        public static event Action<int> onComparissonUpdated;
        public static event Action onSortEnded;

        private static int iterationCount = 0;
        private static int comparissonCount = 0;

        #region SortingMethods

        /// <summary>
        /// Only Works for sizes that are power of 2
        /// Space: O(n log(n))
        /// Time: O(log(n))
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator BitonicSort(List<T> list, float delay)
        {
            yield return RecursiveBitonicSort(list, 0, list.Count, 1, delay);
            onSortEnded?.Invoke();
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

        /// <summary>
        /// Space: O(1)
        /// Time: O(n^2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator SelectionSort(List<T> list, float delay)
        {
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

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(1)
        /// Time:  O(n/2 * n/2) -> O(n^2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator DoubleSelectionSort(List<T> list, float delay)
        {
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

                Swap(list, i, minIndex);

                for (int j = list.Count - 1 - i; j > i; j--)
                {
                    if (Compare(list[j], list[maxIndex]) > 0)
                        maxIndex = j;
                }

                Swap(list, list.Count - 1 - i, maxIndex);
                yield return new WaitForSeconds(delay);
            }

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(1)
        /// Time: O(n^2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator CocktailShakerSort(List<T> list, float delay)
        {
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

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(log(n))
        /// Time: O(n log(n))
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator QuickSort(List<T> list, float delay)
        {
            yield return RecursiveQuickSort(list, 0, list.Count - 1, delay);
            onSortEnded?.Invoke();
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

        /// <summary>
        /// Space: O(n + b) Where b is the quantity of buckets, in other words b is the base of the digits, in this case 10
        /// Time: O(d*(n+b)) where d is the amount of digits
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator RadixLSDSort(List<T> list, float delay)
        {
            List<uint> ints = GetIntsFromT(list);
            uint biggestNum = ints[0];
            for (int i = 1; i < ints.Count; i++)
            {
                if (ints[i] > biggestNum)
                    biggestNum = ints[i];
            }

            int maxDigits = GetNumberOfDigits(Convert.ToInt32(biggestNum));

            uint digitMultiplier = 10;
            List<int>[] buckets = new List<int>[10];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<int>();
            }

            for (int i = 0; i < maxDigits; i++)
            {
                for (int j = 0; j < ints.Count; j++)
                {
                    uint digitResult = ints[j] / (digitMultiplier / 10) % 10;
                    buckets[digitResult].Add(j);
                }

                List<T> auxList = CloneList(list);
                List<uint> auxNumbers = Sort<uint>.CloneList(ints);

                list.Clear();
                ints.Clear();

                for (int bucketIndex = 0; bucketIndex < buckets.Length; bucketIndex++)
                {
                    for (int j = 0; j < buckets[bucketIndex].Count; j++)
                    {
                        list.Add(auxList[buckets[bucketIndex][j]]);
                        ints.Add(auxNumbers[buckets[bucketIndex][j]]);
                        UpdateIterationCount();
                        onListUpdated?.Invoke(list);
                        yield return new WaitForSeconds(delay);
                    }
                }

                digitMultiplier *= 10;
                for (int j = 0; j < buckets.Length; j++)
                    buckets[j].Clear();
            }

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(n+b+d)
        /// Time: O(d*n)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator RadixMSDSort(List<T> list, float delay)
        {
            List<uint> ints = GetIntsFromT(list);
            uint biggestNum = ints[0];
            for (int i = 1; i < ints.Count; i++)
            {
                if (ints[i] > biggestNum)
                    biggestNum = ints[i];
            }

            int maxDigits = GetNumberOfDigits(Convert.ToInt32(biggestNum));
            yield return RecursiveRadixMSD(list, ints, 0, list.Count - 1, maxDigits, delay);
            onSortEnded?.Invoke();
        }

        private static IEnumerator RecursiveRadixMSD(List<T> list, List<uint> ints, int from, int to, int digit, float delay)
        {
            if (to <= from)
                yield break;

            List<int>[] buckets = new List<int>[10];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<int>();
            }

            for (int j = from; j <= to; j++)
            {
                int digitResult = (int)(ints[j] / Mathf.Pow(10, digit - 1) % 10);
                buckets[digitResult].Add(j);
            }

            List<T> auxList = CloneList(list);
            List<uint> auxNumbers = Sort<uint>.CloneList(ints);

            int iterator = from;
            for (int bucketIndex = 0; bucketIndex < buckets.Length; bucketIndex++)
            {
                for (int i = 0; i < buckets[bucketIndex].Count; i++, iterator++)
                {
                    auxList[iterator] = list[buckets[bucketIndex][i]];
                    auxNumbers[iterator] = ints[buckets[bucketIndex][i]];
                }
            }

            for (int i = from; i <= to; i++)
            {
                list[i] = auxList[i];
                ints[i] = auxNumbers[i];
                UpdateIterationCount();
                onListUpdated?.Invoke(list);
                yield return new WaitForSeconds(delay);
            }

            int prevPos = from;
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i].Count < 1)
                    continue;

                yield return RecursiveRadixMSD(list, ints, prevPos, prevPos + buckets[i].Count - 1, digit - 1, delay);
                prevPos += buckets[i].Count;
            }
        }

        /// <summary>
        /// Space: O(1)
        /// Time: O(n^1.2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator ShellSort(List<T> list, float delay)
        {
            for (int gap = (int)(list.Count / 2.3f); gap > 0; gap = (int)(gap / 2.3f))
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

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Randomly Shuffles the list until it is sorted
        /// Space: O(1)
        /// Time: O(n!)
        /// Worst: O(lim->0) 
        /// Best: O(1)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator BogoSort(List<T> list, float delay)
        {
            while (!IsSorted(list))
            {
                Shuffle(list);
                UpdateIterationCount();
                yield return new WaitForSeconds(delay);
            }

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(log(n))
        /// Time: O(n log(n))
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator IntroSort(List<T> list, float delay)
        {
            //Recursive Quantity of steps
            int depthLimit = (int)(2 * Math.Floor(Math.Log(list.Count) / Math.Log(2)));
            yield return RecursiveIntroSort(list, 0, list.Count - 1, depthLimit, delay);
            onSortEnded?.Invoke();
        }

        private static IEnumerator RecursiveIntroSort(List<T> list, int from, int to, int depth, float delay)
        {
            if (to - from > 16)
            {
                if (depth == 0)
                {
                    yield return HeapSort(list, from, to, delay);
                    yield break;
                }

                depth -= 1;

                int middle = from + ((to - from) / 2);
                int pivot = FindPivot(list, from, middle, to);
                Swap(list, pivot, to);

                int divisionIndex = Partition(list, from, to);

                yield return RecursiveIntroSort(list, from, divisionIndex - 1, depth, delay);
                yield return RecursiveIntroSort(list, divisionIndex + 1, to, depth, delay);
            }
            else
            {
                yield return InsertionSort(list, from, to, delay);
            }
        }

        /// <summary>
        /// Space: O(n)
        /// Time: O(n log(n))
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator AdaptiveMergeSort(List<T> list, float delay)
        {
            yield return RecursiveAdaptiveMergeSort(list, 0, list.Count - 1, delay);
            onSortEnded?.Invoke();
        }

        private static IEnumerator RecursiveAdaptiveMergeSort(List<T> list, int from, int to, float delay)
        {
            if (from >= to) yield break;

            int middle = from + (to - from) / 2;

            if (IsSorted(list, from, to, 0))
            {
                Inverse(list, from, to);
            }
            else
            {
                if (IsSorted(list, from, middle, 0))
                    Inverse(list, from, middle);
                if (IsSorted(list, middle + 1, to, 0))
                    Inverse(list, middle + 1, to);
            }

            if (!IsSorted(list, from, to, 1))
            {
                if (!IsSorted(list, from, middle, 1))
                    yield return RecursiveMergeSort(list, from, middle, delay);

                if (!IsSorted(list, middle + 1, to, 1))
                    yield return RecursiveMergeSort(list, middle + 1, to, delay);

                yield return AdaptiveMerge(list, from, middle, to, delay);
            }
        }

        /// <summary>
        /// Space: O(1)
        /// Time: O(n^2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator BubbleSort(List<T> list, float delay)
        {
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

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(1)
        /// Time: O(n^2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator GnomeSort(List<T> list, float delay)
        {
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

            onSortEnded?.Invoke();
        }

        /// <summary>
        /// Space: O(n)
        /// Time: O(n log(n))
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator MergeSort(List<T> list, float delay)
        {
            yield return RecursiveMergeSort(list, 0, list.Count - 1, delay);
            onSortEnded?.Invoke();
        }

        private static IEnumerator RecursiveMergeSort(List<T> list, int from, int to, float delay)
        {
            if (from >= to) yield break;

            int middle = from + (to - from) / 2;

            yield return RecursiveMergeSort(list, from, middle, delay);
            yield return RecursiveMergeSort(list, middle + 1, to, delay);
            yield return Merge(list, from, middle, to, delay);
        }

        /// <summary>
        /// Space: O(1)
        /// Time: O(n log(n))
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator HeapSort(List<T> list, float delay)
        {
            yield return Heapify(list, list.Count - 1, delay);
            for (int i = list.Count - 1; i > 1; i--)
            {
                Swap(list, 0, i);
                yield return new WaitForSeconds(delay);
                yield return Heapify(list, i - 1, delay);
            }

            onSortEnded?.Invoke();
        }

        public static IEnumerator HeapSort(List<T> list, int from, int to, float delay)
        {
            yield return Heapify(list, from, to - 1, delay);
            for (int i = to - 1; i > from; i--)
            {
                Swap(list, 0, i);
                yield return new WaitForSeconds(delay);
                yield return Heapify(list, from, i - 1, delay);
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

        private static IEnumerator Heapify(List<T> list, int from, int lastIndex, float delay)
        {
            if (lastIndex == from)
            {
                if (Compare(list[from], list[0]) < 0)
                    Swap(list, 0, from);
                yield break;
            }

            for (int i = lastIndex; i > from; i -= 2)
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

        /// <summary>
        /// Space: O(1)
        /// Time: O(n^2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator InsertionSort(List<T> list, float delay)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                int j = i - 1;

                while (j >= 0 && Compare(list[i], list[j]) < 0)
                    j--;

                InsertAt(list, i, j + 1);
                yield return new WaitForSeconds(delay);
            }

            onSortEnded?.Invoke();
        }

        public static IEnumerator InsertionSort(List<T> list, int left, int right, float delay)
        {
            for (int i = left; i <= right; ++i)
            {
                int j = i - 1;

                while (j >= left && Compare(list[i], list[j]) < 0)
                    j--;

                InsertAt(list, i, j + 1);
                yield return new WaitForSeconds(delay);
            }
        }

        /// <summary>
        /// Space: O(1) Technically not Space (OS driven)
        /// Time: O(n + arr.max)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static IEnumerator SleepSort(List<T> list, float delay)
        {
            List<uint> ints = GetIntsFromT(list);
            uint biggestNum = ints[0];
            for (int i = 1; i < ints.Count; i++)
            {
                if (ints[i] > biggestNum)
                    biggestNum = ints[i];
            }

            List<T> auxList = CloneList(list);

            Coroutine[] waits = new Coroutine[list.Count];

            list.Clear();
            for (int i = 0; i < waits.Length; i++)
            {
                waits[i] = CoroutineHandler.Instance.StartCoroutine(WaitFor(list, auxList[i], (float)ints[i] / 1000));
            }

            yield return new WaitForSeconds((float)biggestNum / 1000);
            onSortEnded?.Invoke();
        }

        private static IEnumerator WaitFor(List<T> list, T value, float duration)
        {
            Debug.Log(duration);
            yield return new WaitForSeconds(duration);
            list.Add(value);
            UpdateIterationCount();
            onListUpdated?.Invoke(list);
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

        private static bool IsSorted(List<T> list, int from, int to, int dir)
        {
            for (int i = from + 1; i <= to; i++)
            {
                if (dir == 1 && Compare(list[i - 1], list[i]) > 0)
                    return false;

                if (dir == 0 && Compare(list[i - 1], list[i]) < 0)
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

        public static void Shuffle(List<T> list, int from, int to)
        {
            T aux;
            int randomIndex = 0;
            for (int i = from; i < to; i++)
            {
                randomIndex = Random.Range(from, to);
                aux = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = aux;
            }

            onListUpdated?.Invoke(list);
        }

        public static void ResetCounts()
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

        private static IEnumerator AdaptiveMerge(List<T> list, int from, int middle, int to, float delay)
        {
            if (IsSorted(list, from, to, 1))
                yield break;

            yield return Merge(list, from, middle, to, delay);
        }

        private static int FindPivot(List<T> list, int left, int mid, int right)
        {
            int max = left;
            if (Compare(list[left], list[mid]) < 0)
                max = mid;
            if (Compare(list[right], list[mid]) > 0 && Compare(list[right], list[left]) > 0)
                max = right;

            int min = left;
            if (Compare(list[left], list[mid]) > 0)
                min = mid;
            if (Compare(list[right], list[mid]) < 0 && Compare(list[right], list[left]) < 0)
                min = right;

            if ((min == mid && max == right) || (max == mid && min == right))
                return left;
            if ((min == left && max == right) || (max == left && min == right))
                return mid;

            return right;
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

        private static void Inverse(List<T> list, int from, int to)
        {
            for (int i = 0; i < (to - from) / 2 + 1; i++)
            {
                Swap(list, i, to - i);
            }
        }

        private static uint GetIntFromBitArray(List<uint> bits)
        {
            uint result = 0;
            for (int i = bits.Count - 1; i >= 0; i--)
            {
                result *= 2;
                result += bits[i];
            }

            return result;
        }

        private static List<uint> GetIntsFromT(List<T> list)
        {
            List<BitArray> bitArrays = new List<BitArray>();

            List<uint> ints = new List<uint>();
            for (int i = 0; i < list.Count; i++)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, list[i]);
                bitArrays.Add(new BitArray(ms.ToArray()));
            }

            for (int i = 0; i < list.Count; i++)
            {
                List<uint> bits = new List<uint>();
                for (int j = bitArrays[i].Count - 40; j < bitArrays[i].Count - 8; j++)
                {
                    bits.Add(Convert.ToUInt32(bitArrays[i][j]));
                }

                ints.Add(GetIntFromBitArray(bits));
            }

            return ints;
        }

        private static int GetNumberOfDigits(int number)
        {
            if (number < 10)
                return 1;

            return 1 + GetNumberOfDigits(number / 10);
        }

        private static List<T> CloneList(List<T> list)
        {
            List<T> auxList = new List<T>();
            for (int k = 0; k < list.Count; k++)
            {
                auxList.Add(list[k]);
            }

            return auxList;
        }

        #endregion
    }
}