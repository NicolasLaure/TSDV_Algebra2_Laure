using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private MyTransform _transform;
    [SerializeField] private MyTransform _transformTest;


    [ContextMenu("Test")]
    private void Test()
    {
        // Debug.Log(unityTransform.localToWorldMatrix + "\n" + _transform.localToWorldMatrix);
        // Debug.Log(unityTransform.position);
        // Debug.Log(unityTransform.rotation);
        // Debug.Log(unityTransform.lossyScale);

        // _transform.Rotation = MyQuaternion.Euler(45, 12, 0);
        // Debug.Log(_transform.Rotation.eulerAngles);
        // Debug.Log(_transform.transform.rotation.eulerAngles);

       _transformTest = _transform.Find("Cube 2 (2)");

        //Debug.Log(unityTransform.worldToLocalMatrix + "\n" + _transform.WorldToLocalMatrix);
    }
}