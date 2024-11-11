using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private string pinName; // timestamp·Î
    private string videoPath;
    private List<LineObject> memo;

    public Pin (string pinName)
    {
        this.pinName = pinName;
    }


    public string GetPinName()
    {
        return this.pinName;
    }
}
