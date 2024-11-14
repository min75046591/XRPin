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
    public GameObject readUserInterface;
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
        if (this.prevGesture != HandGesture.Pinch && gesture == HandGesture.Pinch) // 이전 제스처가 핀치가 아니고 지금 제스처가 핀치면
        {
            var pose = handState.GetJointPose(HandJointID.IndexTip);
            Vector3 indexTipPosition = pose.position;   // 검지 끝 위치 받아와서
            this.DisablePinGenerationMode();
            Pin pin = pinManager.GeneratePin(indexTipPosition); // 핀 생성
            this.currentLoadedPin.Add(pin); // 현재 표시된 핀에 추가
            this.UseCreatePanel();  // 핀 생성 패널 켜질 수 있게 하고
            this.EnableCreateUserInterface();   // 인터페이스 모드로 변경(핀치 시 그리기모드 <-> 인터페이스 켜지기)
            MenuHover.PassPin(pin); // MenuHover에 핀 정보 보내기
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
        toggle.InitializeCreatePanel();
    }

    public void DisableCreateUserInterface()
    {
        createUserInterface.SetActive(false);
    }
    public void EnableReadUserInterface()
    {
        toggle.InitializeReadPanel();
    }

    public void DisableReadUserInterface()
    {
        readUserInterface.SetActive(false);
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

    public bool IsEnableGenarationMode()
    {
        return this.pinGenerationMode;
    }

    public void UseCreatePanel()
    {
        toggle.UseCreatePanel();
    }
    public void UseReadPanel()
    {
        toggle.UseReadPanel();
    }
}
