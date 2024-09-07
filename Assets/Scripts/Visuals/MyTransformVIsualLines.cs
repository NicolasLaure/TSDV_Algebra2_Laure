using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;


[ExecuteInEditMode]
public class MyTransformVIsualLines : MonoBehaviour
{
    [SerializeField] private MyTransformVisualizer _transformVisualizer;

    [SerializeField] private LineRenderer forwardLine;
    [SerializeField] private LineRenderer rightLine;
    [SerializeField] private LineRenderer upLine;

    private MyTransform _transform;
    private void OnEnable()
    {
        _transform = _transformVisualizer.GetTransform(0);
    }

    private void Update()
    {
        forwardLine.SetPosition(0, _transform.Position);
        forwardLine.SetPosition(1, _transform.Position + _transform.forward);

        rightLine.SetPosition(0, _transform.Position);
        rightLine.SetPosition(1, _transform.Position + _transform.right);

        upLine.SetPosition(0, _transform.Position);
        upLine.SetPosition(1, _transform.Position + _transform.up);
    }
}