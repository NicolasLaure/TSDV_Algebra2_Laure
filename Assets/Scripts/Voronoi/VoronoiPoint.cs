using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

[Serializable]
public class VoronoiPoint
{
    public List<Self_Plane> planes = new List<Self_Plane>();
    public MeshRenderer objectMesh;
}
