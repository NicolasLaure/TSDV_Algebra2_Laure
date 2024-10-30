using System;
using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SortUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;

    private void OnEnable()
    {
        Sort<int>.onCountUpdated += UpdateCount;
    }

    private void OnDisable()
    {
        Sort<int>.onCountUpdated -= UpdateCount;
    }

    private void UpdateCount(int newCount)
    {
        countText.text = "Iteration Count: " + newCount;
    }
}