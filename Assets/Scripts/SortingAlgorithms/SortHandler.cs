using System;
using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using UnityEngine;

public enum SortTypes
{
    Sleep, // O(n + arr.max) -> O(n)
    RadixMSD, // O(d*n)
    RadixLSD, // O(d*(n+b))
    Intro, // O(n log(n))
    Quick, // O(n log(n))
    AdaptiveMerge, // O(n log(n))
    Merge, // O(n log(n))
    Heap, // O(n log(n))
    Bitonic, // O(n*log(n)^2)
    Shell, // O(n^1.2)
    Insertion, // O(n^2)
    DoubleSelection, // O(n^2)
    Selection, // O(n^2)
    CocktailShaker, // O(n^2)
    Gnome, // O(n^2)
    Bubble, // O(n^2)
    Bogo // O(n!)
}

public class SortHandler : MonoBehaviour
{
    [SerializeField] private SortTypes sortType;
    [SerializeField] private float delay;
    [SerializeField] private int quantity;
    [SerializeField] private bool hasAscendantOrder;
    [SerializeField] private bool shouldShuffle;

    [SerializeField] private bool shouldShuffleRange;
    [SerializeField] private int rangeMin;
    [SerializeField] private int rangeMax;
    [SerializeField] private ListVisualizer listVisualizer;

    private Coroutine _sortCoroutine;
    private List<int> _list;

    [ContextMenu("Sort")]
    public void Sort()
    {
        if (_sortCoroutine != null)
            StopCoroutine(_sortCoroutine);

        _list = new List<int>();

        for (int i = 0; i < quantity; i++)
        {
            int number = hasAscendantOrder ? i + 1 : quantity - i;
            _list.Add(number);
        }

        listVisualizer.CreateBars(_list);
        if (shouldShuffle)
            Sort<int>.Shuffle(_list);

        if (shouldShuffleRange)
            Sort<int>.Shuffle(_list, rangeMin, rangeMax);

        Sort<int>.ResetCounts();
        switch (sortType)
        {
            case SortTypes.Bitonic:
                _sortCoroutine = StartCoroutine(Sort<int>.BitonicSort(_list, delay));
                break;
            case SortTypes.Selection:
                _sortCoroutine = StartCoroutine(Sort<int>.SelectionSort(_list, delay));
                break;
            case SortTypes.DoubleSelection:
                _sortCoroutine = StartCoroutine(Sort<int>.DoubleSelectionSort(_list, delay));
                break;
            case SortTypes.CocktailShaker:
                _sortCoroutine = StartCoroutine(Sort<int>.CocktailShakerSort(_list, delay));
                break;
            case SortTypes.Quick:
                _sortCoroutine = StartCoroutine(Sort<int>.QuickSort(_list, delay));
                break;
            case SortTypes.RadixLSD:
                _sortCoroutine = StartCoroutine(Sort<int>.RadixLSDSort(_list, delay));
                break;
            case SortTypes.RadixMSD:
                _sortCoroutine = StartCoroutine(Sort<int>.RadixMSDSort(_list, delay));
                break;
            case SortTypes.Shell:
                _sortCoroutine = StartCoroutine(Sort<int>.ShellSort(_list, delay));
                break;
            case SortTypes.Bogo:
                _sortCoroutine = StartCoroutine(Sort<int>.BogoSort(_list, delay));
                break;
            case SortTypes.Intro:
                _sortCoroutine = StartCoroutine(Sort<int>.IntroSort(_list, delay));
                break;
            case SortTypes.AdaptiveMerge:
                _sortCoroutine = StartCoroutine(Sort<int>.AdaptiveMergeSort(_list, delay));
                break;
            case SortTypes.Bubble:
                _sortCoroutine = StartCoroutine(Sort<int>.BubbleSort(_list, delay));
                break;
            case SortTypes.Gnome:
                _sortCoroutine = StartCoroutine(Sort<int>.GnomeSort(_list, delay));
                break;
            case SortTypes.Merge:
                _sortCoroutine = StartCoroutine(Sort<int>.MergeSort(_list, delay));
                break;
            case SortTypes.Heap:
                _sortCoroutine = StartCoroutine(Sort<int>.HeapSort(_list, delay));
                break;
            case SortTypes.Insertion:
                _sortCoroutine = StartCoroutine(Sort<int>.InsertionSort(_list, delay));
                break;
            case SortTypes.Sleep:
                _sortCoroutine = StartCoroutine(Sort<int>.SleepSort(_list, delay));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}