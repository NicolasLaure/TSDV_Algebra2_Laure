using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using UnityEngine;

public class SortHandler : MonoBehaviour
{
    [SerializeField] private List<int> testList;

    [ContextMenu("IsSorted")]
    private void IsSorted()
    {
       Debug.Log($"Is testList Sorted? {Sort.IsSorted(testList)}");
    }
}