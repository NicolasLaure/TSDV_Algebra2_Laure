using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    public static CoroutineHandler Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Coroutine HandleStartCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}