using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    [SerializeField] private float x, y, z;
    public Point(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Point(Vector3 p)
    {
        this.x = p.x;
        this.y = p.y;
        this.z = p.z;
    }

    public Vector3 PointToVector3()
    {
        return new Vector3(this.x, this.y, this.z);
    }
}
