using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PinManager : MonoBehaviour
{
    public GameObject generatedPinPrefab;
    public GameObject workingPinPrefab;
    public GameObject completedPinPrefab;
    public Transform pinTransform;

    public Pin GeneratePin(Vector3 indexTipPosition)
    {
        string pinName = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.fff");
        Pin pin = new Pin(pinName);
        pin.SetPinPoint(indexTipPosition);
        pin.ChangePinStatus(PinStatus.Generated);
        return pin;
    }

    public void DisplayPin(Pin pin)
    {
        GameObject currentPinPrefab = JudgePinPrefab(pin.GetPinStatus());
        pinTransform.position = pin.GetPinPoint();
        Debug.Log("pin position: " + pinTransform.position);
        GameObject newPin = Instantiate(currentPinPrefab, pinTransform);
        newPin.name = pin.GetPinName();
        newPin.SetActive(true);
        newPin.transform.SetParent(null);
        newPin = null;
    }

    public void ChangePinStatusIntoWorking(Pin pin)
    {
        pin.ChangePinStatus(PinStatus.Working);
    }
    public void ChangePinStatusIntoCompleted(Pin pin)
    {
        pin.ChangePinStatus(PinStatus.Completed);
    }

    public void DestroyPInObject(Pin pin)
    {
        GameObject pinObject = GameObject.Find(pin.GetPinName());
        if (pinObject != null)
        {
            Destroy(pinObject);
        }
    }

    private GameObject JudgePinPrefab(PinStatus pinStatus)
    {
        switch (pinStatus)
        {
            case PinStatus.Generated:
                return generatedPinPrefab;
            case PinStatus.Working:
                return workingPinPrefab;
            case PinStatus.Completed:
                return completedPinPrefab;
            default:
                return generatedPinPrefab;
        }
    }
}
