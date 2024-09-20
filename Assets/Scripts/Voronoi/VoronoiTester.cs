using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class VoronoiTester : MonoBehaviour
{
    [SerializeField] private GameObject pointGameObject;
    [SerializeField] private Voronoi voronoi;
    // Update is called once per frame

    [SerializeField] private Material defaultMat;
    [SerializeField] private Material highLightMat;

    private VoronoiPoint lastPoint;

    void Update()
    {
        UpdateClosestPoint();
    }

    private void UpdateClosestPoint()
    {
        VoronoiPoint newVoronoiPoint = voronoi.GetClosestPoint(new Vec3(pointGameObject.transform.position));
        if (lastPoint == null)
            lastPoint = newVoronoiPoint;

        else if (lastPoint != newVoronoiPoint)
        {
            lastPoint.objectMesh.material = defaultMat;
            lastPoint = newVoronoiPoint;
        }

        lastPoint.objectMesh.material = highLightMat;
    }
}