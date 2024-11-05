using UnityEngine;
using UnityEngine.UI;
using NRKernal;

public class MenuHover : MonoBehaviour
{
    public HandEnum handEnum;
    public RectTransform panelRectTransform; // Panel의 RectTransform
    public Image targetImage; // 포인터가 올려져야 할 이미지 (예: pen 버튼)
    public float hoverTime = 1f; // 포인터가 머무르는 시간 (1초)
    public Camera nrealCamera; // Nreal 카메라를 명시적으로 지정

    public GameObject[] thickButtons; // thick1, thick2, thick3 버튼 배열

    private float hoverTimer = 0f;
    private bool isHovering = false;

    void Start()
    {
        HideThickButtons(); // thick 버튼은 초기에는 숨김
    }

    void Update()   
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        Debug.Log($"Hand IndexTip World Position: {pose.position}");
        Vector3 screenPoint = nrealCamera.WorldToScreenPoint(pose.position);
        Debug.Log($"Hand IndexTip Screen Position: {screenPoint}");

        // 스크린 좌표가 유효한지 확인 (Z값이 양수인 경우만)
        if (screenPoint.z > 0)
        {
            Vector2 pointerScreenPos = new Vector2(screenPoint.x, screenPoint.y);

            // 손끝이 Panel 내부에 있는지 확인
            isHovering = RectTransformUtility.RectangleContainsScreenPoint(
                    panelRectTransform, new Vector2(screenPoint.x, screenPoint.y), nrealCamera);
            Debug.Log($"Is Hovering Panel: {isHovering}");


            Debug.Log($"Panel RectTransform Position: {panelRectTransform.position}, Size: {panelRectTransform.rect.size}");
            Debug.Log($"Target Image RectTransform Position: {targetImage.rectTransform.position}, Size: {targetImage.rectTransform.rect.size}");

            // 포인터가 Panel의 특정 버튼 위에 있는지 확인하여 타이머 증가
            if (isHovering && RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
            {
                hoverTimer += Time.deltaTime;

                // 지정된 시간 이상 Hover 시 thick 버튼 활성화
                if (hoverTimer >= hoverTime)
                {
                    ShowThickButtons();
                }
            }
            else
            {
                // 포인터가 Panel에서 벗어나면 타이머 초기화 및 thick 버튼 숨기기
                hoverTimer = 0f;
                HideThickButtons();
            }
        }
    }

    void ShowThickButtons()
    {
        foreach (var button in thickButtons)
        {
            button.SetActive(true); // thick1, thick2, thick3 버튼 활성화
        }
        Debug.Log("Thick buttons are now visible");
    }

    void HideThickButtons()
    {
        foreach (var button in thickButtons)
        {
            button.SetActive(false); // thick1, thick2, thick3 버튼 비활성화
        }
    }
}
