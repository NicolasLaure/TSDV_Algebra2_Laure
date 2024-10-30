using System;
using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ListVisualizer : MonoBehaviour
{
    [SerializeField] private float width;
    [SerializeField] private GameObject barPrefab;
    private List<GameObject> _bars = new List<GameObject>();
    private float _barWidth = 0;
    
    private void OnEnable()
    {
        Sort<int>.onListUpdated += UpdateBars;
    }

    private void OnDisable()
    {
        Sort<int>.onListUpdated -= UpdateBars;
    }

    public void CreateBars(List<int> list)
    {
        for (int i = 0; i < _bars.Count; i++)
        {
            Destroy(_bars[i].gameObject);
        }

        _bars.Clear();

        _barWidth = width / list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject bar = Instantiate(barPrefab, transform);
            bar.transform.localScale = new Vector3(_barWidth, list[i], 1);
            bar.transform.position = transform.position + new Vector3(_barWidth / 2 + _barWidth * i, bar.transform.localScale.y / 2, 0);
            _bars.Add(bar);
        }
    }

    private void UpdateBars(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            _bars[i].transform.localScale = new Vector3(_barWidth, list[i], 1);
            _bars[i].transform.position = new Vector3(_bars[i].transform.position.x, transform.position.y + _bars[i].transform.localScale.y / 2, _bars[i].transform.position.z);
        }
    }
}