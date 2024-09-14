using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

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

    [SerializeField] private bool shouldRotateAround = false;


    private Vec3 _pivotPos = Vec3.Zero;
    private MyQuaternion _pivotRotation = MyQuaternion.identity;
    private Vec3 _pivotScale = Vec3.Zero;

    [SerializeField] private List<Vector3> directions = new List<Vector3>();

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

        _pivotPos = new Vec3(pivotUnityTransform.position);
        _pivotRotation = new MyQuaternion(pivotUnityTransform.rotation);
        _pivotScale = new Vec3(pivotUnityTransform.localScale);

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

        //Works
        //TransformDirection simple
        // Debug.Log(unityTransform.TransformDirection(dir));
        // Debug.Log(visualizer.GetTransform(1).TransformDirection(new Vec3(dir)));

        //Works
        //TransformDirection arrays
        // TestTransformDirections();

        //Works
        //InverseTransformDirection
        // Debug.Log(unityTransform.InverseTransformDirection(dir));
        // Debug.Log(visualizer.GetTransform(1).InverseTransformDirection(new Vec3(dir)));

        //Works
        //InverseTransformDirections
        //TestInverseTransformDirections();

        //Works
        //TransformVector
        // Debug.Log(unityTransform.TransformVector(dir));
        // Debug.Log(visualizer.GetTransform(1).TransformVector(new Vec3(dir)));

        //Works
        //TransformVectors
        // TestTransformVectors();

        //Works
        //InverseTransformVector
        // Debug.Log(unityTransform.InverseTransformVector(dir));
        // Debug.Log(visualizer.GetTransform(1).InverseTransformVector(new Vec3(dir)));

        //Works
        //InverseTransformVectors
        //TestInverseTransformVectors();

        //Works
        //TransformPoint
        // Debug.Log(unityTransform.TransformPoint(dir));
        // Debug.Log(visualizer.GetTransform(1).TransformPoint(new Vec3(dir)));

        //Works
        //TransformPoints
        //TestTransformPoints();

        //Works As I expect not same with Unity
        //InverseTransformPoint
        Debug.Log(unityTransform.InverseTransformPoint(position));
        unityTransform.localPosition = unityTransform.InverseTransformPoint(position);
        Debug.Log(visualizer.GetTransform(1).InverseTransformPoint(new Vec3(position)));
        visualizer.GetTransform(1).LocalPosition = visualizer.GetTransform(1).InverseTransformPoint(new Vec3(position));

        //InverseTransformPoints
        //TestInverseTransformPoints();

        //Debug.Log(unityTransform.worldToLocalMatrix + "\n" + _transform.WorldToLocalMatrix);
    }

    private void Update()
    {
        _pivotPos = new Vec3(pivotUnityTransform.position);
        _pivotRotation = new MyQuaternion(pivotUnityTransform.rotation);
        _pivotScale = new Vec3(pivotUnityTransform.localScale);

        if (shouldRotateAround)
        {
            unityTransform.RotateAround(pivotUnityTransform.position, axis, angle * Time.deltaTime);
            visualizer.GetTransform(1).RotateAround(_pivotPos, new Vec3(axis), angle * Time.deltaTime);
        }
    }

    private void TestTransformDirections()
    {
        List<Vec3> v3Directions = new List<Vec3>();
        foreach (Vector3 direction in directions)
        {
            v3Directions.Add(new Vec3(direction));
        }

        Span<Vector3> directionsPtr = new Span<Vector3>(directions.ToArray());
        Span<Vector3> transformedDirections = new Span<Vector3>(directions.ToArray());

        unityTransform.TransformDirections(directionsPtr, transformedDirections);
        foreach (Vector3 direction in transformedDirections)
        {
            Debug.Log(direction);
        }

        Span<Vec3> vec3DirectionsPtr = new Span<Vec3>(v3Directions.ToArray());
        Span<Vec3> vec3TransformedDirections = new Span<Vec3>(v3Directions.ToArray());

        visualizer.GetTransform(1).TransformDirections(vec3DirectionsPtr, vec3TransformedDirections);
        Debug.Log("MyTransform\n\n\n\n");
        foreach (Vec3 direction in vec3TransformedDirections)
        {
            Debug.Log(direction);
        }
    }

    private void TestInverseTransformDirections()
    {
        List<Vec3> v3Directions = new List<Vec3>();
        foreach (Vector3 direction in directions)
        {
            v3Directions.Add(new Vec3(direction));
        }

        Span<Vector3> directionsPtr = new Span<Vector3>(directions.ToArray());
        Span<Vector3> transformedDirections = new Span<Vector3>(directions.ToArray());

        unityTransform.InverseTransformDirections(directionsPtr, transformedDirections);
        foreach (Vector3 direction in transformedDirections)
        {
            Debug.Log(direction);
        }

        Span<Vec3> vec3DirectionsPtr = new Span<Vec3>(v3Directions.ToArray());
        Span<Vec3> vec3TransformedDirections = new Span<Vec3>(v3Directions.ToArray());

        visualizer.GetTransform(1).InverseTransformDirections(vec3DirectionsPtr, vec3TransformedDirections);
        Debug.Log("MyTransform\n\n\n\n");
        foreach (Vec3 direction in vec3TransformedDirections)
        {
            Debug.Log(direction);
        }
    }

    private void TestTransformVectors()
    {
        List<Vec3> v3Directions = new List<Vec3>();
        foreach (Vector3 direction in directions)
        {
            v3Directions.Add(new Vec3(direction));
        }

        Span<Vector3> directionsPtr = new Span<Vector3>(directions.ToArray());
        Span<Vector3> transformedDirections = new Span<Vector3>(directions.ToArray());

        unityTransform.TransformVectors(directionsPtr, transformedDirections);
        foreach (Vector3 direction in transformedDirections)
        {
            Debug.Log(direction);
        }

        Span<Vec3> vec3DirectionsPtr = new Span<Vec3>(v3Directions.ToArray());
        Span<Vec3> vec3TransformedDirections = new Span<Vec3>(v3Directions.ToArray());

        visualizer.GetTransform(1).TransformVectors(vec3DirectionsPtr, vec3TransformedDirections);
        Debug.Log("MyTransform\n\n\n\n");
        foreach (Vec3 direction in vec3TransformedDirections)
        {
            Debug.Log(direction);
        }
    }

    private void TestInverseTransformVectors()
    {
        List<Vec3> v3Directions = new List<Vec3>();
        foreach (Vector3 direction in directions)
        {
            v3Directions.Add(new Vec3(direction));
        }

        Span<Vector3> directionsPtr = new Span<Vector3>(directions.ToArray());
        Span<Vector3> transformedDirections = new Span<Vector3>(directions.ToArray());

        unityTransform.InverseTransformVectors(directionsPtr, transformedDirections);
        foreach (Vector3 direction in transformedDirections)
        {
            Debug.Log(direction);
        }

        Span<Vec3> vec3DirectionsPtr = new Span<Vec3>(v3Directions.ToArray());
        Span<Vec3> vec3TransformedDirections = new Span<Vec3>(v3Directions.ToArray());

        visualizer.GetTransform(1).InverseTransformVectors(vec3DirectionsPtr, vec3TransformedDirections);
        Debug.Log("MyTransform\n\n\n\n");
        foreach (Vec3 direction in vec3TransformedDirections)
        {
            Debug.Log(direction);
        }
    }

    private void TestTransformPoints()
    {
        List<Vec3> v3Directions = new List<Vec3>();
        foreach (Vector3 direction in directions)
        {
            v3Directions.Add(new Vec3(direction));
        }

        Span<Vector3> directionsPtr = new Span<Vector3>(directions.ToArray());
        Span<Vector3> transformedDirections = new Span<Vector3>(directions.ToArray());

        unityTransform.TransformPoints(directionsPtr, transformedDirections);
        foreach (Vector3 direction in transformedDirections)
        {
            Debug.Log(direction);
        }

        Span<Vec3> vec3DirectionsPtr = new Span<Vec3>(v3Directions.ToArray());
        Span<Vec3> vec3TransformedDirections = new Span<Vec3>(v3Directions.ToArray());

        visualizer.GetTransform(1).TransformPoints(vec3DirectionsPtr, vec3TransformedDirections);
        Debug.Log("MyTransform\n\n\n\n");
        foreach (Vec3 direction in vec3TransformedDirections)
        {
            Debug.Log(direction);
        }
    }

    private void TestInverseTransformPoints()
    {
        List<Vec3> v3Directions = new List<Vec3>();
        foreach (Vector3 direction in directions)
        {
            v3Directions.Add(new Vec3(direction));
        }

        Span<Vector3> directionsPtr = new Span<Vector3>(directions.ToArray());
        Span<Vector3> transformedDirections = new Span<Vector3>(directions.ToArray());

        unityTransform.InverseTransformPoints(directionsPtr, transformedDirections);
        foreach (Vector3 direction in transformedDirections)
        {
            Debug.Log(direction);
        }

        Span<Vec3> vec3DirectionsPtr = new Span<Vec3>(v3Directions.ToArray());
        Span<Vec3> vec3TransformedDirections = new Span<Vec3>(v3Directions.ToArray());

        visualizer.GetTransform(1).InverseTransformPoints(vec3DirectionsPtr, vec3TransformedDirections);
        Debug.Log("MyTransform\n\n\n\n");
        foreach (Vec3 direction in vec3TransformedDirections)
        {
            Debug.Log(direction);
        }
    }
}