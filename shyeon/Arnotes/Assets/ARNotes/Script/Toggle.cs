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
            Debug.Log("test");
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
        InitializingStationeryController();
    }
    public void InitializeReadPanel()
    {
        this.isPanelOpen = true;
        this.readPanel.SetActive(true);
        InitializingStationeryController();
    }

    public void UseReadPanel()
    {
        this.currentUsedPanel = readPanel;
    }
    public void UseCreatePanel()
    {
        this.currentUsedPanel = createPanel;
    }

    private void InitializingStationeryController()
    {
        this.stationeryContrller.SetActive(true);
        this.stationeryContrller.SetActive(false);
    }
}
