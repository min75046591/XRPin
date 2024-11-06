using UnityEngine;
using UnityEngine.UI;
using NRKernal;

public class MenuHover : MonoBehaviour
{
    public HandEnum handEnum;
    public RectTransform panelRectTransform; // Panel�� RectTransform
    public Image targetImage; // �����Ͱ� �÷����� �� �̹��� (��: pen ��ư)
    public float hoverTime = 1f; // ���� ��ư�� ȣ�� �ð� (1��)
    public float buttonHoverTime = 0.5f; // ���� ��ư�� ȣ�� �ð� (0.5��)
    public Camera nrealCamera; // Nreal ī�޶� ���������� ����

    public GameObject[] thickButtons; // thick1, thick2, thick3 ��ư �迭

    private float hoverTimer = 0f;
    private bool isHovering = false;
    private GameObject currentHoveredButton = null; // ���� ȣ�� ���� ���� ��ư
    private float buttonHoverTimer = 0f;
    private Color originalTargetColor; // targetImage�� ���� ���� ����

    void Start()
    {
        // thick ��ư�� �ʱ⿡�� ����
        HideThickButtons(); 
        if (targetImage != null)
        {
            // targetImage�� ���� ���� ����
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

        // Buttons�� Ȱ��ȭ�� ������ ��
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
        // Panel ���ο� �ճ��� �ִ��� Ȯ��
        isHovering = RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, pointerScreenPos, nrealCamera);

        if (isHovering && RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        {
            hoverTimer += Time.deltaTime;

            // Panel���� ������ �ð� �̻� ȣ�� �� ��ư Ȱ��ȭ
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
                // �ٸ� ��ư�� �����ϸ� Ÿ�̸� �ʱ�ȭ
                if (currentHoveredButton != button)
                {
                    currentHoveredButton = button;
                    buttonHoverTimer = 0f;
                }

                buttonHoverTimer += Time.deltaTime;

                // ���� ��ư���� ������ �ð� �̻� ȣ�� �� ��ư ��Ȱ��ȭ �� ���� �̹��� ���� ����
                if (buttonHoverTimer >= buttonHoverTime)
                {
                    Debug.Log($"{button.name} ��ư�� ���õǾ����ϴ�.");

                    // ���� �̹��� ���� ����
                    ResetColor(targetImage, originalTargetColor); 
                    HideThickButtons();
                }
                return;
            }
        }

        // ���� ��ư���� ����� �ʱ�ȭ
        buttonHoverTimer = 0f;
        currentHoveredButton = null;
    }

    void ShowThickButtons()
    {
        foreach (var button in thickButtons)
        {
            //���� ������Ʈ button�� Ȱ��ȭ
            button.SetActive(true); 
        }
        Debug.Log("Thick buttons are now visible");
    }

    void HideThickButtons()
    {
        foreach (var button in thickButtons)
        {
            //���� ������Ʈ button�� ��Ȱ��ȭ
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
