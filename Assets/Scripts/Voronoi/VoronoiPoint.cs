using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class VoronoiPoint
{
    public List<Self_Plane> planes = new List<Self_Plane>();
    public List<GameObject> planeGameObject = new List<GameObject>();
    public List<Vec3> planePositions = new List<Vec3>();
    public MeshRenderer objectMesh;
}