//using UnityEngine;
//using NRKernal;

//public class FingerMenuInteraction : MonoBehaviour
//{
//    public HandEnum handEnum;
//    public GameObject[] menus; // 여러 개의 메뉴 오브젝트 배열
//    public float tolerance = 5f; // 위치 비교를 위한 허용 오차 (화면 좌표 상에서의 거리)
//    private GameObject lastExecutedMenu; // 마지막으로 실행된 메뉴를 추적

//    void Update()
//    {
//        if (!NRInput.Hands.IsRunning)
//            return;

//        var handState = NRInput.Hands.GetHandState(handEnum);
//        var pose = handState.GetJointPose(HandJointID.IndexTip);

//        // 손가락 끝의 화면 좌표 (x, y) 가져오기
//        Vector3 fingerScreenPos = Camera.main.WorldToScreenPoint(pose.position);

//        // 각 메뉴의 화면 좌표 (x, y)와 손가락 끝 좌표를 비교
//        foreach (var menu in menus)
//        {
//            Vector3 menuScreenPos = Camera.main.WorldToScreenPoint(menu.transform.position);

//            // x, y 좌표가 tolerance 범위 안에 있는지 확인 (z축 무시)
//            if (Mathf.Abs(Mathf.Abs(fingerScreenPos.x) - menuScreenPos.x) <= tolerance &&
//                Mathf.Abs(Mathf.Abs(fingerScreenPos.y) - menuScreenPos.y) <= tolerance)
//            {
//                // 메뉴가 이미 실행 중이 아니거나, 마지막 실행된 메뉴와 다르면 실행
//                if (lastExecutedMenu != menu)
//                {
//                    ExecuteMenu(menu);
//                    lastExecutedMenu = menu; // 마지막으로 실행한 메뉴 업데이트
//                }
//            }
//            else if (lastExecutedMenu == menu)
//            {
//                // 손가락이 메뉴를 떠난 경우 초기화
//                lastExecutedMenu = null;
//            }
//        }
//    }

//    void ExecuteMenu(GameObject menu)
//    {
//        // 메뉴 실행 로직 (예: 해당 메뉴 활성화 또는 특정 기능 실행)
//        menu.SetActive(true);
//        Debug.Log($"{menu.name} Menu Executed!");
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using NRKernal;

public class ImageHoverMenu : MonoBehaviour
{
    public HandEnum handEnum;
    public Image targetImage; // 포인터가 올려져야 할 이미지
    public GameObject menu; // 보여줄 메뉴 오브젝트
    public float hoverTime = 1f; // 포인터가 머무르는 시간 (1초)

    private float hoverTimer = 0f;
    private bool isHovering = false;

    void Start()
    {
        menu.SetActive(false); // 메뉴는 초기에는 숨겨둠
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        // 컨트롤러 위치의 화면 좌표 가져오기
        Vector3 pointerScreenPos = Camera.main.WorldToScreenPoint(pose.position);

        // PointerEventData 생성 (컨트롤러의 화면 좌표를 기반으로)
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = pointerScreenPos
        };

        // Raycast 결과 리스트 생성
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // 이미지 위에 포인터가 있는지 확인
        isHovering = false;
        foreach (var result in results)
        {
            if (result.gameObject == targetImage.gameObject)
            {
                isHovering = true;
                break;
            }
        }

        // 포인터가 이미지 위에 있는지 확인하여 타이머 증가
        if (isHovering)
        {
            hoverTimer += Time.deltaTime;

            // 지정된 시간 이상 Hover 시 메뉴 활성화
            if (hoverTimer >= hoverTime)
            {
                ShowMenu();
            }
        }
        else
        {
            // 포인터가 이미지에서 벗어나면 타이머 초기화 및 메뉴 숨기기
            hoverTimer = 0f;
            menu.SetActive(false);
        }
    }

    void ShowMenu()
    {
        menu.SetActive(true);
        Debug.Log("Menu is now visible");
    }
}
