using System;
using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SortUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iterationCountText;
    [SerializeField] private TextMeshProUGUI comparissonsCountText;

    private void OnEnable()
    {
        Sort<int>.oniterationCountUpdated += UpdateIterationCount;
        Sort<int>.onComparissonUpdated += UpdateComparissonsCount;
    }

    private void OnDisable()
    {
        Sort<int>.oniterationCountUpdated -= UpdateIterationCount;
        Sort<int>.onComparissonUpdated -= UpdateComparissonsCount;
    }

    private void UpdateIterationCount(int newCount)
    {
        iterationCountText.text = "Iteration Count: " + newCount;
    }
    
    private void UpdateComparissonsCount(int newCount)
    {
        comparissonsCountText.text = "Comparissons Count: " + newCount;
    }
}