using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class InterfaceToggle : MonoBehaviour
{
    public HandEnum handEnum;
    public GameObject panel; // ���� ���� �г� ������Ʈ
    public GameObject stationeryContrller;
    private bool isPanelOpen = false; // �г��� �ʱ� ����
    private bool wasPinching = false; // ���� �����ӿ��� Pinch ���¿����� ����

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
            panel.SetActive(isPanelOpen);
            stationeryContrller.SetActive(!isPanelOpen);
        }

        // ���� Pinch ���¸� wasPinching�� ������Ʈ
        wasPinching = (handState.currentGesture == HandGesture.Pinch);
    }
}
