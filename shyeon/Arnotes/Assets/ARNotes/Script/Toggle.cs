using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class InterfaceToggle : MonoBehaviour
{
    public HandEnum handEnum;
    public GameObject createPanel; // ���� ���� �г� ������Ʈ
    public GameObject readPanel;
    public GameObject stationeryContrller;
    public GameObject UICursor;
    private bool isPanelOpen = false; // �г��� �ʱ� ����
    private bool wasPinching = false; // ���� �����ӿ��� Pinch ���¿����� ����
    private GameObject currentUsedPanel;


    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;
        var handState = NRInput.Hands.GetHandState(handEnum);

        // Pinch ����ó�� ó�� �߻����� ���� ���
        if (handState.currentGesture == HandGesture.Pinch && !wasPinching)
        {
            // �г��� ���� Ȱ�� ���¸� �ݴ�� ����
            isPanelOpen = !isPanelOpen;
            currentUsedPanel.SetActive(isPanelOpen);
            stationeryContrller.SetActive(!isPanelOpen);
            UICursor.SetActive(isPanelOpen);
        }

        // ���� Pinch ���¸� wasPinching�� ������Ʈ
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
