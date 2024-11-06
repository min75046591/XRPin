using UnityEngine;
using UnityEngine.UI;
using NRKernal;

public class MenuHover : MonoBehaviour
{
    public HandEnum handEnum;
    public RectTransform panelRectTransform; // Panel의 RectTransform
    public Image targetImage; // 포인터가 올려져야 할 이미지 (예: pen 버튼)
    public float hoverTime = 1f; // 상위 버튼의 호버 시간 (1초)
    public float buttonHoverTime = 0.5f; // 하위 버튼의 호버 시간 (0.5초)
    public Camera nrealCamera; // Nreal 카메라를 명시적으로 지정

    public GameObject[] thickButtons; // thick1, thick2, thick3 버튼 배열

    private float hoverTimer = 0f;
    private bool isHovering = false;
    private GameObject currentHoveredButton = null; // 현재 호버 중인 하위 버튼
    private float buttonHoverTimer = 0f;
    private Color originalTargetColor; // targetImage의 원래 색상 저장

    void Start()
    {
        // thick 버튼은 초기에는 숨김
        HideThickButtons(); 
        if (targetImage != null)
        {
            // targetImage의 원래 색상 저장
            originalTargetColor = targetImage.color; 
        }
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        Vector3 screenPoint = nrealCamera.WorldToScreenPoint(pose.position);
        Vector2 pointerScreenPos = new Vector2(screenPoint.x, screenPoint.y);

        // Buttons가 활성화된 상태일 때
        if (thickButtons[0].activeSelf) 
        {
            CheckHoverOnThickButtons(pointerScreenPos);
        }
        else
        {
            CheckHoverOnPanel(pointerScreenPos);
        }
    }

    void CheckHoverOnPanel(Vector2 pointerScreenPos)
    {
        // Panel 내부에 손끝이 있는지 확인
        isHovering = RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, pointerScreenPos, nrealCamera);

        if (isHovering && RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        {
            hoverTimer += Time.deltaTime;

            // Panel에서 지정된 시간 이상 호버 시 버튼 활성화
            if (hoverTimer >= hoverTime)
            {
                InvertColor(targetImage);
                ShowThickButtons();
            }
        }
        else
        {
            hoverTimer = 0f;
            HideThickButtons();
        }
    }

    void CheckHoverOnThickButtons(Vector2 pointerScreenPos)
    {
        foreach (var button in thickButtons)
        {
            RectTransform buttonRect = button.GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(buttonRect, pointerScreenPos, nrealCamera))
            {
                // 다른 버튼에 진입하면 타이머 초기화
                if (currentHoveredButton != button)
                {
                    currentHoveredButton = button;
                    buttonHoverTimer = 0f;
                }

                buttonHoverTimer += Time.deltaTime;

                // 하위 버튼에서 지정된 시간 이상 호버 시 버튼 비활성화 및 상위 이미지 색상 리셋
                if (buttonHoverTimer >= buttonHoverTime)
                {
                    Debug.Log($"{button.name} 버튼이 선택되었습니다.");

                    // 상위 이미지 색상 리셋
                    ResetColor(targetImage, originalTargetColor); 
                    HideThickButtons();
                }
                return;
            }
        }

        // 하위 버튼에서 벗어나면 초기화
        buttonHoverTimer = 0f;
        currentHoveredButton = null;
    }

    void ShowThickButtons()
    {
        foreach (var button in thickButtons)
        {
            //하위 오브젝트 button들 활성화
            button.SetActive(true); 
        }
        Debug.Log("Thick buttons are now visible");
    }

    void HideThickButtons()
    {
        foreach (var button in thickButtons)
        {
            //하위 오브젝트 button들 비활성화
            button.SetActive(false); 
        }
        hoverTimer = 0f;
        buttonHoverTimer = 0f;
        currentHoveredButton = null;
        isHovering = false;
    }

    void InvertColor(Image image)
    {
        if (image != null)
        {
            Color invertedColor = new Color(1 - image.color.r, 1 - image.color.g, 1 - image.color.b, image.color.a);
            image.color = invertedColor;
        }
    }

    void ResetColor(Image image, Color originalColor)
    {
        if (image != null)
        {
            image.color = originalColor;
        }
    }
}
