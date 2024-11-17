using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanelController : MonoBehaviour
{
    public Text pinCountText;  // PinCountText ������Ʈ�� ����� Text ������Ʈ
    public int currentPinCount;

    public MainController mainController;

    void Update()
    {
        // �ʱ� �� ������ ǥ��
        UpdatePinCountDisplay();
    }

    // �� ���� UI�� ������Ʈ�ϴ� �޼���
    public void UpdatePinCountDisplay()
    {
       
        if (pinCountText != null)
        {
            currentPinCount = mainController.GetCurrentNonCompletedPin();
            pinCountText.text = "��Ϸ� �� ��: " + currentPinCount;
            
        }
        else
        {
            Debug.LogWarning("PinCountText�� ������� �ʾҽ��ϴ�.");
        }
    }
}
