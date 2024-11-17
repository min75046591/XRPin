using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using UnityEngine.UI;

public class MemoSaver : MonoBehaviour
{
    public HandEnum handEnum;
    public SaveLineRenderer saveLineRender;
    public JsonManager jsonManager;
    public StationeryController stationeryController;
    public InterfaceToggle toggle;

    private Pin currentPin;

    void Update()
    {
        if (!NRInput.Hands.IsRunning) return;
        var handState = NRInput.Hands.GetHandState(handEnum);
        if (handState.currentGesture == HandGesture.Pinch)
        {
            
            currentPin.SetMemos(saveLineRender.GetLineObject());
            jsonManager.Save(currentPin);
            stationeryController.RemoveAll();
            gameObject.SetActive(false);
            toggle.InitializeReadPanel();
        }
    }

    public void SetCurrentPin(Pin pin)
    {
        this.currentPin = pin;
    }
}
