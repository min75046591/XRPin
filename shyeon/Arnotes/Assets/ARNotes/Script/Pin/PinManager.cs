using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PinManager : MonoBehaviour
{
    public GameObject pinPrefab;
    public Transform pinTransform;

    public Pin GeneratePin(Vector3 indexTipPosition)
    {
        string pinName = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.fff");
        Pin pin = new Pin(pinName);
        pin.SetPinPoint(indexTipPosition);
        DisplayPin(pin);
        return pin;
    }

   public void DisplayPin(Pin pin)
    {
        pinTransform.position = pin.GetPinPoint();
        GameObject newPin = Instantiate(pinPrefab, pinTransform);
        newPin.SetActive(true);
        newPin.transform.SetParent(null);
        newPin = null;
    }

}
