using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using System;
using UnityEngine.SceneManagement;



public class PinManager : MonoBehaviour
{
    public static PinManager instance { get; private set; }
    public GameObject pinPrefab;
    public Transform pinTransform;
    public HandEnum handEnum;
    private List<Pin> currentLoadedPin = new List<Pin>();
    private HandGesture prevGesture;

    void Update()
    {
        if (!NRInput.Hands.IsRunning) return;
        var handState = NRInput.Hands.GetHandState(handEnum);
        JudgePinGeneration(handState);
        this.prevGesture = handState.currentGesture;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Pin> GetPins()
    {
        return currentLoadedPin;
    }

    private void JudgePinGeneration(HandState handState)
    {
        HandGesture gesture = handState.currentGesture;
        if (this.prevGesture != HandGesture.Pinch && gesture == HandGesture.Pinch)
        {
            var pose = handState.GetJointPose(HandJointID.IndexTip);
            Vector3 indexTipPosition = pose.position;
            GeneratePin(indexTipPosition);
        }
    }

    private void GeneratePin(Vector3 indexTipPosition)
    {
        pinTransform.position = indexTipPosition;
        GameObject newPin = Instantiate(pinPrefab, pinTransform);
        newPin.SetActive(true);
        newPin.transform.SetParent(null);
        newPin = null;
        string pinName = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        Pin pinInfo = new Pin(pinName);
        currentLoadedPin.Add(pinInfo);
        SceneManager.LoadScene("ARNotes");
    }

}
