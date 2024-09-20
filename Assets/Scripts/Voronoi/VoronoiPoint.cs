using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VoronoiPoint
{
    public List<Vector3> normals = new List<Vector3>();
    public MeshRenderer objectMesh;
}
