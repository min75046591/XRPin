using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleControllerBar : MonoBehaviour
{
    public GameObject uiElement; // UI 요소를 지정합니다.
    public Transform penpoint; // 컨트롤러 오브젝트를 지정합니다.
    public float showThreshold = 2.0f; // UI가 보이기 시작할 Y 위치

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        // UI 요소에 붙은 CanvasGroup 컴포넌트를 가져옵니다.
        canvasGroup = uiElement.GetComponent<CanvasGroup>();

        // 초기 상태를 숨김으로 설정
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 컨트롤러의 Y 위치가 showThreshold 이상일 때 UI를 표시
        if (penpoint.position.y > showThreshold)
        {
            canvasGroup.alpha = 1; // UI 보이기
        }
        else
        {
            canvasGroup.alpha = 0; // UI 숨기기
        }
    }
}
