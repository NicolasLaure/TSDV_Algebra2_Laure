using System;
using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using UnityEngine;

public enum SortTypes
{
    Bitonic,
    Selection,
    CocktailShaker,
    Quick,
    RadixLSD,
    RadixMSD,
    Shell,
    Bogo,
    Intro,
    AdaptiveMerge,
    Bubble,
    Gnome,
    Merge,
    Heap,
    Insertion
}

public class SortHandler : MonoBehaviour
{
    [SerializeField] private SortTypes sortType;
    [SerializeField] private float delay;
    [SerializeField] private float quantity;

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
            _list.Add(i + 1);
        }

        listVisualizer.CreateBars(_list);
        Sort<int>.Shuffle(_list);
        switch (sortType)
        {
            case SortTypes.Bitonic:
                break;
            case SortTypes.Selection:
                break;
            case SortTypes.CocktailShaker:
                break;
            case SortTypes.Quick:
                break;
            case SortTypes.RadixLSD:
                break;
            case SortTypes.RadixMSD:
                break;
            case SortTypes.Shell:
                break;
            case SortTypes.Bogo:
                _sortCoroutine = StartCoroutine(Sort<int>.BogoSort(_list, delay));
                break;
            case SortTypes.Intro:
                break;
            case SortTypes.AdaptiveMerge:
                break;
            case SortTypes.Bubble:
                _sortCoroutine = StartCoroutine(Sort<int>.BubbleSort(_list, delay));
                break;
            case SortTypes.Gnome:
                break;
            case SortTypes.Merge:
                break;
            case SortTypes.Heap:
                break;
            case SortTypes.Insertion:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}