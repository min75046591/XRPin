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

    private MenuHover menuHover = new MenuHover();
    private HandGesture prevGesture;
    private List<Pin> currentLoadedPin = new List<Pin>();

    private bool pinGenerationMode = true;


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
        if(pinGenerationMode) JudgePinGeneration(handState);
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
    }

    public void DisablePinGenerationMode()
    {
        this.pinGenerationMode = false;
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
}
