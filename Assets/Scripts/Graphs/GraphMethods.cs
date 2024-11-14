using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphMethods
{
    /// <summary>
    /// Determines whether all elements of a sequence satisfy a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool All<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();
        while (sourceEnum.MoveNext())
        {
            if (!predicate.Invoke(sourceEnum.Current))
            {
                sourceEnum.Dispose();
                return false;
            }
        }

        sourceEnum.Dispose();
        return true;
    }

    /// <summary>
    /// Determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool Any<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();
        while (sourceEnum.MoveNext())
        {
            if (predicate.Invoke(sourceEnum.Current))
            {
                sourceEnum.Dispose();
                return true;
            }
        }

        sourceEnum.Dispose();
        return false;
    }

    /// <summary>
    /// Determines whether a sequence contains a specified element by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool Contains<TSource>(IEnumerable<TSource> source, TSource item)
    {
        return Contains(source, item, EqualityComparer<TSource>.Default);
    }

    /// <summary>
    /// Determines whether a sequence contains a specified element by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool Contains<TSource>(IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();
        while (sourceEnum.MoveNext())
        {
            if (comparer.Equals(sourceEnum.Current, item))
            {
                sourceEnum.Dispose();
                return true;
            }
        }

        sourceEnum.Dispose();
        return false;
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source)
    {
        return Distinct(source, EqualityComparer<TSource>.Default);
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using a specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
        List<TSource> result = new List<TSource>();
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();

        while (sourceEnum.MoveNext())
        {
            if (result.Count == 0)
                result.Add(sourceEnum.Current);

            bool isDistinct = true;
            for (int i = 0; i < result.Count; i++)
            {
                if (comparer.Equals(sourceEnum.Current, result[i]))
                {
                    isDistinct = false;
                    break;
                }
            }

            if (isDistinct)
                result.Add(sourceEnum.Current);
        }

        sourceEnum.Dispose();
        return result;
    }

    /// <summary>
    /// Returns the element at a specified index in a sequence.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static TSource ElementAt<TSource>(IEnumerable<TSource> source, int index)
    {
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();
        int currentIndex = -1;
        while (sourceEnum.MoveNext())
        {
            currentIndex++;

            if (currentIndex == index)
            {
                sourceEnum.Dispose();
                return sourceEnum.Current;
            }
        }

        sourceEnum.Dispose();
        throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Produces the set difference of two sequences by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        return Except(source1, source2, EqualityComparer<TSource>.Default);
    }

    /// <summary>
    /// Produces the set difference of two sequences by using the specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        IEnumerator<TSource> source1Enum = source1.GetEnumerator();
        List<TSource> excepts = new List<TSource>();

        while (source1Enum.MoveNext())
        {
            if (!Contains(source2, source1Enum.Current))
                excepts.Add(source1Enum.Current);
        }

        source1Enum.Dispose();

        return excepts;
    }

    /// <summary>
    /// Returns the first element in a sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource First<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();
        int count = -1;
        while (sourceEnum.MoveNext())
        {
            count++;
            if (predicate.Invoke(sourceEnum.Current))
            {
                sourceEnum.Dispose();
                Debug.Log(count);
                return sourceEnum.Current;
            }
        }

        sourceEnum.Dispose();
        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Returns the last element of a sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource Last<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        IEnumerator<TSource> sourceEnum = source.GetEnumerator();
        int count = -1;
        int lastFoundIndex = -1;
        while (sourceEnum.MoveNext())
        {
            count++;
            if (predicate.Invoke(sourceEnum.Current))
                lastFoundIndex = count;
        }

        sourceEnum.Dispose();
        Debug.Log(lastFoundIndex);
        if (lastFoundIndex != -1)
            return ElementAt(source, lastFoundIndex);

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Produces the set intersection of two sequences by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Intersect<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        return Intersect(source1, source2, EqualityComparer<TSource>.Default);
    }

    /// <summary>
    /// Produces the set intersection of two sequences by using the specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Intersect<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        List<TSource> intersectionList = new List<TSource>();
        IEnumerator<TSource> source1Enum = source1.GetEnumerator();
        while (source1Enum.MoveNext())
        {
            if (Contains(source2, source1Enum.Current))
                intersectionList.Add(source1Enum.Current);
        }

        source1Enum.Dispose();
        return intersectionList;
    }

    /// <summary>
    /// Returns a number that represents how many elements in the specified sequence satisfy a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int Count<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether two sequences are equal by comparing their elements by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool SequenceEqual<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource Single<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> SkipWhile<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Produces the set union of two sequences by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Produces the set union of two sequences by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public static List<TSource> ToList<TSource>(IEnumerable<TSource> source)
    {
        List<TSource> list = new List<TSource>();
        foreach (TSource data in source)
        {
            list.Add(data);
        }

        return list;
    }
}