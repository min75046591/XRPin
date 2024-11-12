using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineObject
{
    [SerializeField] private List<Point> points = new List<Point>();
    [SerializeField] private float lineWidth;
    [SerializeField] private Color color;
    

    public void AddPoint(float x, float y, float z)
    {
        this.points.Add(new Point(x, y, z));
    }
}
