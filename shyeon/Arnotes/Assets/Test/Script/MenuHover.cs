using UnityEngine;
using NRKernal;

public class FingerMenuInteraction : MonoBehaviour
{
    public HandEnum handEnum;
    public GameObject[] menus; // 여러 개의 메뉴 오브젝트 배열
    public float tolerance = 50f; // 위치 비교를 위한 허용 오차 (화면 좌표 상에서의 거리)
    private GameObject lastExecutedMenu; // 마지막으로 실행된 메뉴를 추적

    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        // 손가락 끝의 화면 좌표 (x, y) 가져오기
        Vector3 fingerScreenPos = Camera.main.WorldToScreenPoint(pose.position);

        // 각 메뉴의 화면 좌표 (x, y)와 손가락 끝 좌표를 비교
        foreach (var menu in menus)
        {
            Vector3 menuScreenPos = Camera.main.WorldToScreenPoint(menu.transform.position);

            // x, y 좌표가 tolerance 범위 안에 있는지 확인 (z축 무시)
            if (Mathf.Abs(fingerScreenPos.x - menuScreenPos.x) <= tolerance &&
                Mathf.Abs(Mathf.Abs(fingerScreenPos.y) - menuScreenPos.y) <= tolerance)
            {
                // 메뉴가 이미 실행 중이 아니거나, 마지막 실행된 메뉴와 다르면 실행
                if (lastExecutedMenu != menu)
                {
                    ExecuteMenu(menu);
                    lastExecutedMenu = menu; // 마지막으로 실행한 메뉴 업데이트
                }
            }
            else if (lastExecutedMenu == menu)
            {
                // 손가락이 메뉴를 떠난 경우 초기화
                lastExecutedMenu = null;
            }
        }
    }

    void ExecuteMenu(GameObject menu)
    {
        // 메뉴 실행 로직 (예: 해당 메뉴 활성화 또는 특정 기능 실행)
        menu.SetActive(true);
        Debug.Log($"{menu.name} Menu Executed!");
    }
}
