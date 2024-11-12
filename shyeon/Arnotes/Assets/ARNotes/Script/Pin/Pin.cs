using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pin
{
    [SerializeField] private Point pinPoint;
    [SerializeField] private string pinName; // timestamp·Î
    [SerializeField] private string videoPath;
    [SerializeField] private List<LineObject> memo = new List<LineObject>();

    public Pin (string pinName)
    {
        this.pinName = pinName;
    }


    public string GetPinName()
    {
        return this.pinName;
    }

    public void AddLine(LineObject line)
    {
        this.memo.Add(line);
    }

    public void SetPinPoint(Vector3 p)
    {
        this.pinPoint = new Point(p);
    }

    public Vector3 PinPointtoVector3()
    {
        return pinPoint.PointToVector3();
    }
}
