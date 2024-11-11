using UnityEngine;
using UnityEngine.UI;
using NRKernal;
using Unity.VisualScripting;

public class MenuHover : MonoBehaviour
{
    public MenuCommander menuCommander;
    public HandEnum handEnum;
    public RectTransform panelRectTransform;
    public Transform panel;
    public Image targetImage;
    public static Image currentImage;
    public float hoverTime = 0.5f;
    public float buttonHoverTime = 0.5f;
    public Camera nrealCamera;
    public GameObject[] thickButtons;
    public float speed = 1f;
    public Image SubTargetImage;


    private float hoverTimer = 0f;
    private bool isHovering = false;
    private GameObject currentHoveredButton = null;
    private float buttonHoverTimer = 0f;
    private Color originalTargetColor;
    private Vector3 initialSize = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 scaledSize = new Vector3(1.2f, 1.2f, 1.2f);


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

        if(targetImage.name == "record")
        {
            Debug.Log("record-image 호출");
        }

        if (thickButtons[0].activeSelf && targetImage.name != "record") // 하위 메뉴가 켜지면
        {
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
        targetImage.transform.localScale = Vector3.Lerp(initialSize, scaledSize, hoverTimer * 3f);
        isHovering = RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, pointerScreenPos, nrealCamera);
        //if (isHovering && RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        if (RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverTime)
            {
                Debug.Log(targetImage.transform.localScale);
                //if (currentImage != null && currentImage != targetImage)
                //{
                //    hoverTimer -= Time.deltaTime * 3f;
                //    if (hoverTimer <= 0) hoverTimer = 0;
                //    HideThickButtonsUnderImage(currentImage);
                //}
                hoverTimer = 0;
                ShowThickButtons();
                if (currentImage == null)
                { currentImage = targetImage; }
            }
        }
        else
        {
            hoverTimer = 0;
            //hoverTimer -= Time.deltaTime * 3f;
            //if (hoverTimer <= 0) hoverTimer = 0;
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
                button.transform.localScale = Vector3.Lerp(initialSize, scaledSize, buttonHoverTimer * 2f);

                if (currentHoveredButton != button)
                {
                    currentHoveredButton = button;
                    buttonHoverTimer = 0f;
                }

                buttonHoverTimer += Time.deltaTime;

                if (buttonHoverTimer >= buttonHoverTime)
                {
                    Debug.Log(button);
                    ResetColor(SubTargetImage, originalTargetColor);
                    menuCommander.Command(button.name);
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
        InvertColor(SubTargetImage);

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
        //hoverTimer = 0f;
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
        currentImage = null;
        ResetColor(SubTargetImage, originalTargetColor);

        foreach (var button in thickButtons)
        {
            button.SetActive(false);
        }
    }
}
