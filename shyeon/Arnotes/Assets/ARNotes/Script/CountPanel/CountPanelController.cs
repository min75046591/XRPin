using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanelController : MonoBehaviour
{
    public Text pinCountText;  // PinCountText 오브젝트와 연결된 Text 컴포넌트
    public int currentPinCount;

    public MainController mainController;

    void Update()
    {
        // 초기 핀 개수를 표시
        UpdatePinCountDisplay();
    }

    // 핀 개수 UI를 업데이트하는 메서드
    public void UpdatePinCountDisplay()
    {
       
        if (pinCountText != null)
        {
            currentPinCount = mainController.GetCurrentNonCompletedPin();
            pinCountText.text = "비완료 핀 수: " + currentPinCount;
            
        }
        else
        {
            Debug.LogWarning("PinCountText가 연결되지 않았습니다.");
        }
    }
}
