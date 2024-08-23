using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private MyTransform _transform;


    [ContextMenu("Test")]
    private void Test()
    {
        // Debug.Log(unityTransform.localToWorldMatrix + "\n" + _transform.localToWorldMatrix);
        // Debug.Log(unityTransform.position);
        // Debug.Log(unityTransform.rotation);
        // Debug.Log(unityTransform.lossyScale);

        _transform.Position = new Vec3(10, 0, 0);
        //Debug.Log(unityTransform.worldToLocalMatrix + "\n" + _transform.WorldToLocalMatrix);
    }
}