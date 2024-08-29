using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;


[ExecuteInEditMode]
public class MyTransformVIsualLines : MonoBehaviour
{
    [SerializeField] private MyTransform _transform;

    [SerializeField] private LineRenderer forwardLine;
    [SerializeField] private LineRenderer rightLine;
    [SerializeField] private LineRenderer upLine;

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