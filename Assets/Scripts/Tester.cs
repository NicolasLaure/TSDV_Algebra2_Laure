using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using UnityEngine.Serialization;

public class Tester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private MyTransform _transform;

    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 eulers;

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

        unityTransform.SetPositionAndRotation(position, Quaternion.Euler(eulers));
        _transform.SetPositionAndRotation(new Vec3(position), MyQuaternion.Euler(new Vec3(eulers)));

        //_transformTest = _transform.Find("Cube 2 (2)");

        //Debug.Log(unityTransform.worldToLocalMatrix + "\n" + _transform.WorldToLocalMatrix);
    }
}