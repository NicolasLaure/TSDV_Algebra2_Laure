using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using UnityEngine.Rendering;

public class VoronoiSpatialTessellation : MonoBehaviour
{
    [SerializeField] private List<GameObject> staticObjects = new List<GameObject>();
    private List<Self_Plane> _planes = new List<Self_Plane>();

    [SerializeField] private GameObject planePrefab;

    private List<VoronoiPoint> _voronoiObjects;

    private void Start()
    {
        _voronoiObjects = new List<VoronoiPoint>();

        for (int i = 0; i < staticObjects.Count; i++)
        {
            _voronoiObjects.Add(new VoronoiPoint());
            _voronoiObjects[i].objectMesh = staticObjects[i].GetComponent<MeshRenderer>();

            foreach (GameObject point in staticObjects)
            {
                if (staticObjects[i] == point)
                    continue;

                Vec3 dir = new Vec3((staticObjects[i].transform.position - point.transform.position).normalized);
                Vec3 position = Vec3.Lerp(new Vec3(staticObjects[i].transform.position), new Vec3(point.transform.position), 0.5f);
                Self_Plane newPlane = new Self_Plane(dir, position);

                _planes.Add(newPlane);
                GameObject newPlaneObject = Instantiate(planePrefab, position, Quaternion.identity);
                newPlaneObject.name = "Plane" + _planes.Count;
                newPlaneObject.transform.up = newPlane.Normal;


                _voronoiObjects[i].planePositions.Add(position);
                _voronoiObjects[i].planeGameObject.Add(newPlaneObject);
                _voronoiObjects[i].planes.Add(newPlane);
            }
        }

        foreach (VoronoiPoint point in _voronoiObjects)
        {
            CleanPlanes(point);
        }
    }

    public VoronoiPoint GetClosestPoint(Vec3 point)
    {
        bool isPointOut = false;
        foreach (VoronoiPoint voronoiPoint in _voronoiObjects)
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

            return voronoiPoint;
        }

        return null;
    }

    private void CleanPlanes(VoronoiPoint voronoiPoint)
    {
        List<Self_Plane> planesToDelete = new List<Self_Plane>();
        List<GameObject> planesGameObjectsToDelete = new List<GameObject>();

        for (int i = 0; i < voronoiPoint.planePositions.Count; i++)
        {
            for (int j = 0; j < voronoiPoint.planes.Count; j++)
            {
                if (i != j)
                    if (!voronoiPoint.planes[j].GetSide(voronoiPoint.planePositions[i]))
                    {
                        planesToDelete.Add(voronoiPoint.planes[i]);
                        planesGameObjectsToDelete.Add(voronoiPoint.planeGameObject[i]);
                        break;
                    }
            }
        }

        for (int i = 0; i < planesToDelete.Count; i++)
        {
            voronoiPoint.planes.Remove(planesToDelete[i]);
            Destroy(planesGameObjectsToDelete[i]);
            voronoiPoint.planeGameObject.Remove(planesGameObjectsToDelete[i]);
        }
    }
}