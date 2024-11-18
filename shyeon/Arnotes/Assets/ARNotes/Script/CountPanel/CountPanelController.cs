using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanelController : MonoBehaviour
{
    public Text pinCountText;  // PinCountText 오브젝트와 연결된 Text 컴포넌트
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
        // 초기 핀 개수를 표시
        UpdatePinCountDisplay();
    }

    // 핀 개수 UI를 업데이트하는 메서드
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
