using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class InterfaceToggle : MonoBehaviour
{
    public HandEnum handEnum;
    public GameObject createPanel; // 열고 닫을 패널 오브젝트
    public GameObject readPanel;
    public GameObject stationeryContrller;
    public GameObject UICursor;
    private bool isPanelOpen = false; // 패널의 초기 상태
    private bool wasPinching = false; // 이전 프레임에서 Pinch 상태였는지 추적
    private GameObject currentUsedPanel;


    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;
        var handState = NRInput.Hands.GetHandState(handEnum);

        // Pinch 제스처가 처음 발생했을 때만 토글
        if (handState.currentGesture == HandGesture.Pinch && !wasPinching)
        {
            // 패널의 현재 활성 상태를 반대로 설정
            isPanelOpen = !isPanelOpen;
            currentUsedPanel.SetActive(isPanelOpen);
            stationeryContrller.SetActive(!isPanelOpen);
            UICursor.SetActive(isPanelOpen);
        }

        // 현재 Pinch 상태를 wasPinching에 업데이트
        wasPinching = (handState.currentGesture == HandGesture.Pinch);
    }

    public void InitializeCreatePanel()
    {
        this.isPanelOpen = false;
        gameObject.SetActive(true);
        InitializeStationeryController();
    }
    public void InitializeReadPanel()
    {
        this.readPanel.SetActive(true);
        gameObject.SetActive(false);
        InitializeStationeryController();
    }

    
    public void UseCreatePanel()
    {
        this.currentUsedPanel = createPanel;
    }

    public void EnableStationery()
    {
        this.readPanel.SetActive(false);
        this.stationeryContrller.SetActive(true);
    }

    private void InitializeStationeryController()
    {
        this.stationeryContrller.SetActive(true);
        this.stationeryContrller.SetActive(false);
    }
}
