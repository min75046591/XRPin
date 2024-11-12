using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class MainController : MonoBehaviour
{
    public HandEnum handEnum;
    public PinManager pinManager;
    public GameObject pinManagerGameObject;
    public JsonManager jsonManager;

    private HandGesture prevGesture;
    private List<Pin> currentLoadedPin = new List<Pin>();


    void Start()
    {
        this.currentLoadedPin = this.jsonManager.LoadAll();
        foreach(Pin p in currentLoadedPin)
        {
            pinManager.DisplayPin(p);
        }
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning) return;
        var handState = NRInput.Hands.GetHandState(handEnum);
        JudgePinGeneration(handState);
        this.prevGesture = handState.currentGesture;
    }
    private void JudgePinGeneration(HandState handState)
    {
        HandGesture gesture = handState.currentGesture;
        if (this.prevGesture != HandGesture.Pinch && gesture == HandGesture.Pinch)
        {
            var pose = handState.GetJointPose(HandJointID.IndexTip);
            Vector3 indexTipPosition = pose.position;
            Pin pin = pinManager.GeneratePin(indexTipPosition);
            LineObject line = new LineObject();
            this.currentLoadedPin.Add(pin);
            this.pinManagerGameObject.SetActive(false);
            this.jsonManager.Save(pin);
        }
    }
}
