using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pin
{
    [SerializeField] private Vector3 pinPoint;
    [SerializeField] private string pinName; // timestamp·Î
    [SerializeField] private string videoPath;
    [SerializeField] private List<LineObject> memo = new List<LineObject>();
    [SerializeField] private PinStatus pinStatus;

    public Pin(string pinName)
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

    public string getVideoPaths()
    {
        return this.videoPath;
    }

    public void setVideoPath(string videoPath)
    {
        this.videoPath = videoPath;
    }

    public void ChangePinStatus(PinStatus changeStatus)
    {
        this.pinStatus = changeStatus;
    }


    public List<LineObject> GetMemos()
    {
        return this.memo;
    }

    public void SetMemos(List<LineObject> lines)
    {
        this.memo = lines;
    }

    public void SetPinPoint(Vector3 p)
    {
        this.pinPoint = p;
    }

    public Vector3 GetPinPoint()
    {
        return this.pinPoint;
    }

    public PinStatus GetPinStatus()
    {
        return this.pinStatus;
    }
}
