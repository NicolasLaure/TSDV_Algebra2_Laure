using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using UnityEngine.Serialization;

public class Tester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private MyTransformVisualizer visualizer;

    [SerializeField] private Transform pivotUnityTransform;

    [SerializeField] private Transform targetUnity;

    [SerializeField] private Vector3 dir;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 eulers;

    [SerializeField] private Vector3 axis;
    [SerializeField] private float angle;

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

        //unityTransform.SetPositionAndRotation(position, Quaternion.Euler(eulers));
        //_transform.SetPositionAndRotation(new Vec3(position), MyQuaternion.Euler(new Vec3(eulers)));

        Vec3 pivotPos = new Vec3(pivotUnityTransform.position);
        MyQuaternion pivotRotation = new MyQuaternion(pivotUnityTransform.rotation);
        Vec3 pivotScale = new Vec3(pivotUnityTransform.localScale);

        //Works
        // Translate
        // unityTransform.Translate(dir, pivotUnityTransform);
        // visualizer.GetTransform(1).Translate(new Vec3(dir), new MyTransform("Pivot", pivotPos, pivotRotation, pivotScale));

        //Works
        //Rotate based on Eulers
        // unityTransform.Rotate(eulers);
        // visualizer.GetTransform(1).Rotate(new Vec3(eulers));

        //Works
        // //Rotate based on Axis
        // unityTransform.Rotate(axis, angle, Space.Self);
        // visualizer.GetTransform(1).Rotate(new Vec3(axis), angle, Space.Self);

        
        //Debug.Log(unityTransform.worldToLocalMatrix + "\n" + _transform.WorldToLocalMatrix);
    }
}