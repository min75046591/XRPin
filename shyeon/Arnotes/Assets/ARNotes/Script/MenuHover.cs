using UnityEngine;
using UnityEngine.UI;
using NRKernal;

public class MenuHover : MonoBehaviour
{
    public HandEnum handEnum;
    public RectTransform panelRectTransform;
    public Transform panel;
    public Image targetImage;
    public static Image currentImage;
    public float hoverTime = 1f;
    public float buttonHoverTime = 0.5f;
    public Camera nrealCamera;

    public GameObject[] thickButtons;

    private float hoverTimer = 0f;
    private bool isHovering = false;
    private GameObject currentHoveredButton = null;
    private float buttonHoverTimer = 0f;
    private Color originalTargetColor;

    void Start()
    {
        HideThickButtons();
        if (targetImage != null)
        {
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

        if (thickButtons[0].activeSelf) // 하위 메뉴가 켜지면
        {
            Debug.Log($"#####################currentImage: {currentImage}");
            if (currentImage != null && currentImage != targetImage)
            { HideThickButtonsUnderImage(currentImage); }
            CheckHoverOnThickButtons(pointerScreenPos); // 하위메뉴를 선택하는 창 관리
        }
        else
        {
            CheckHoverOnPanel(pointerScreenPos); // 상위 메뉴를 여는 창
        }
    }

    void CheckHoverOnPanel(Vector2 pointerScreenPos)
    {
        isHovering = RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, pointerScreenPos, nrealCamera);

        //if (isHovering && RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        if (RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        {
            hoverTimer += Time.deltaTime;

            if (hoverTimer >= hoverTime)
            {
                if (currentImage != null && currentImage != targetImage)
                { 
                    HideThickButtonsUnderImage(currentImage);
                }
                ShowThickButtons();
                if (currentImage == null)
                { currentImage = targetImage; }
                
                
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
                if (currentHoveredButton != button)
                {
                    currentHoveredButton = button;
                    buttonHoverTimer = 0f;
                }

                buttonHoverTimer += Time.deltaTime;

                if (buttonHoverTimer >= buttonHoverTime)
                {
                    Debug.Log($"{button.name}을 실행합니다.");

                    ResetColor(targetImage, originalTargetColor);
                    HideThickButtons();
                }
                return;
            }
        }

        buttonHoverTimer = 0f;
        currentHoveredButton = null;
    }

    void ShowThickButtons()
    {
        InvertColor(targetImage);

        foreach (var button in thickButtons)
        {
            button.SetActive(true);
        }
        Debug.Log("Thick buttons are now visible");
    }

    void HideThickButtons()
    {
        foreach (var button in thickButtons)
        {
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

    void HideThickButtonsUnderImage(Image parentImage)
    {
        Debug.Log("HideThick 함수 실행");
        currentImage = null;
        Debug.Log("currentImage 초기화");
        ResetColor(targetImage, originalTargetColor);

        foreach (var button in thickButtons)
        {
            Debug.Log($"{button}");
            button.SetActive(false);
        }
    }
}
