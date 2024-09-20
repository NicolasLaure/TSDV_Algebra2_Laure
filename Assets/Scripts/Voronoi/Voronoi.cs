using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    [SerializeField] private List<GameObject> staticObjects = new List<GameObject>();
    private List<Plane> _planes = new List<Plane>();

    [SerializeField] private GameObject planePrefab;


    private void Start()
    {

        for (int i = 0; i < staticObjects.Count; i++)
        {
            foreach (GameObject point in staticObjects)
            {
                if (staticObjects[i] == point)
                    continue;

                Vector3 dir = (staticObjects[i].transform.position - point.transform.position).normalized;
                Vector3 position = Vector3.Lerp(staticObjects[i].transform.position, point.transform.position, 0.5f);
                _planes.Add(new Plane(dir, position));
                GameObject newPlane = Instantiate(planePrefab, position, Quaternion.identity);
                newPlane.transform.up = dir;
            }
        }
    }

}