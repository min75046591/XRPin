using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanelController : MonoBehaviour
{
    public Text pinCountText;  // PinCountText ������Ʈ�� ����� Text ������Ʈ
    public Text timeText;
    public int currentPinCount;
    private float startTime;

    public MainController mainController;

    private void Start()
    {
        startTime = Time.time;
    }


    void Update()
    {
        // �ʱ� �� ������ ǥ��
        UpdatePinCountDisplay();
    }

    // �� ���� UI�� ������Ʈ�ϴ� �޼���
    public void UpdatePinCountDisplay()
    {
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timeText.text = $": {minutes}m {seconds}s ";

        currentPinCount = mainController.GetCurrentNonCompletedPin();
        pinCountText.text = $": {currentPinCount}";

    }
}
