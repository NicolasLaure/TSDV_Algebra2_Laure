using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VoronoiPoint
{
    public List<Plane> planes = new List<Plane>();
    public MeshRenderer objectMesh;
}
