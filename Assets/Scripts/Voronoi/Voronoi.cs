using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    [SerializeField] private List<GameObject> staticObjects = new List<GameObject>();
    private List<Plane> _planes = new List<Plane>();

    [SerializeField] private GameObject planePrefab;

    private List<VoronoiPoint> _voronoiObject;

    private void Start()
    {
        _voronoiObject = new List<VoronoiPoint>();

        for (int i = 0; i < staticObjects.Count; i++)
        {
            _voronoiObject.Add(new VoronoiPoint());
            _voronoiObject[i].objectMesh = staticObjects[i].GetComponent<MeshRenderer>();

            foreach (GameObject point in staticObjects)
            {
                if (staticObjects[i] == point)
                    continue;

                Vector3 dir = (staticObjects[i].transform.position - point.transform.position).normalized;
                Vector3 position = Vector3.Lerp(staticObjects[i].transform.position, point.transform.position, 0.5f);
                Plane newPlane = new Plane(dir, position);
                _planes.Add(newPlane);
                GameObject newPlaneObject = Instantiate(planePrefab, position, Quaternion.identity);
                newPlaneObject.transform.up = dir;

                _voronoiObject[i].planes.Add(newPlane);
            }
        }
    }

    public VoronoiPoint GetClosestPoint(Vector3 point)
    {
        bool isPointOut = false;
        foreach (VoronoiPoint voronoiPoint in _voronoiObject)
        {
            isPointOut = false;
            for (int i = 0; i < voronoiPoint.planes.Count; i++)
            {
                if (!voronoiPoint.planes[i].GetSide(point))
                {
                    isPointOut = true;
                    continue;
                }
            }

            if (isPointOut)
                continue;
            else
                return voronoiPoint;
        }

        return null;
    }
}