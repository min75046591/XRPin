using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class MainController : MonoBehaviour
{
    public HandEnum handEnum;
    public PinManager pinManager;
    public JsonManager jsonManager; 
    public GameObject createUserInterface;
    public InterfaceToggle toggle;
    public GameObject indexTip;

    private MenuHover menuHover = new MenuHover();
    private HandGesture prevGesture;
    private List<Pin> currentLoadedPin = new List<Pin>();

    private bool pinGenerationMode = true;


    void Start()
    {
        this.currentLoadedPin = this.jsonManager.LoadAll();
        for(int i = 0; i < this.currentLoadedPin.Count;i++)
        {
            pinManager.DisplayPin(this.currentLoadedPin[i]);
        }
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning) return;
        var handState = NRInput.Hands.GetHandState(handEnum);
        if (pinGenerationMode)
        {
            JudgePinGeneration(handState);
            this.indexTip.transform.position = handState.GetJointPose(HandJointID.IndexTip).position;
        }
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
            this.currentLoadedPin.Add(pin);
            this.DisablePinGenerationMode();
            this.EnableCreateUserInterface();
            MenuHover.PassPin(pin);

        }
    }

    public void EnablePinGenerationMode()
    {
        this.pinGenerationMode = true;
        this.indexTip.SetActive(true);
    }

    public void DisablePinGenerationMode()
    {
        this.pinGenerationMode = false;
        this.indexTip.SetActive(false);
    }

    public void EnableCreateUserInterface()
    {
        toggle.InitializeToggle();
        createUserInterface.SetActive(true);
     
    }

    public void DisableCreateUserInterface()
    {
        createUserInterface.SetActive(false);
    }

    public List<Pin> GetCurrentLoadedPin()
    {
        return this.currentLoadedPin;
    }

    public Pin FindPinByName(string pinName)
    {
        foreach(Pin pin in currentLoadedPin)
        {
            if (pin.GetPinName() == pinName) return pin;
        }
        return null;
    }
}
