using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class VoronoiSpatialTessellation : MonoBehaviour
{
    [SerializeField] private List<GameObject> staticObjects = new List<GameObject>();
    private List<Self_Plane> _planes = new List<Self_Plane>();

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

                Vec3 dir = new Vec3((staticObjects[i].transform.position - point.transform.position).normalized);
                Vec3 position = Vec3.Lerp(new Vec3(staticObjects[i].transform.position), new Vec3(point.transform.position), 0.5f);
                Self_Plane newPlane = new Self_Plane(dir, position);
                _planes.Add(newPlane);
                GameObject newPlaneObject = Instantiate(planePrefab, newPlane.Normal * newPlane.Distance, Quaternion.identity);
                newPlaneObject.transform.up = newPlane.Normal;

                _voronoiObject[i].planes.Add(newPlane);
            }
        }
    }

    public VoronoiPoint GetClosestPoint(Vec3 point)
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