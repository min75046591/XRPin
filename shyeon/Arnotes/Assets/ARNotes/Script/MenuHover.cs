using UnityEngine;
using UnityEngine.UI;
using NRKernal;

public class MenuHover : MonoBehaviour
{
    public HandEnum handEnum;
    public RectTransform panelRectTransform; // Panel�� RectTransform
    public Image targetImage; // �����Ͱ� �÷����� �� �̹��� (��: pen ��ư)
    public float hoverTime = 1f; // �����Ͱ� �ӹ����� �ð� (1��)
    public Camera nrealCamera; // Nreal ī�޶� ��������� ����

    public GameObject[] thickButtons; // thick1, thick2, thick3 ��ư �迭

    private float hoverTimer = 0f;
    private bool isHovering = false;

    void Start()
    {
        HideThickButtons(); // thick ��ư�� �ʱ⿡�� ����
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

        // ��ũ�� ��ǥ�� ��ȿ���� Ȯ�� (Z���� ����� ��츸)
        if (screenPoint.z > 0)
        {
            Vector2 pointerScreenPos = new Vector2(screenPoint.x, screenPoint.y);

            // �ճ��� Panel ���ο� �ִ��� Ȯ��
            isHovering = RectTransformUtility.RectangleContainsScreenPoint(
                    panelRectTransform, new Vector2(screenPoint.x, screenPoint.y), nrealCamera);
            Debug.Log($"Is Hovering Panel: {isHovering}");


            Debug.Log($"Panel RectTransform Position: {panelRectTransform.position}, Size: {panelRectTransform.rect.size}");
            Debug.Log($"Target Image RectTransform Position: {targetImage.rectTransform.position}, Size: {targetImage.rectTransform.rect.size}");

            // �����Ͱ� Panel�� Ư�� ��ư ���� �ִ��� Ȯ���Ͽ� Ÿ�̸� ����
            if (isHovering && RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
            {
                hoverTimer += Time.deltaTime;

                // ������ �ð� �̻� Hover �� thick ��ư Ȱ��ȭ
                if (hoverTimer >= hoverTime)
                {
                    ShowThickButtons();
                }
            }
            else
            {
                // �����Ͱ� Panel���� ����� Ÿ�̸� �ʱ�ȭ �� thick ��ư �����
                hoverTimer = 0f;
                HideThickButtons();
            }
        }
    }

    void ShowThickButtons()
    {
        foreach (var button in thickButtons)
        {
            button.SetActive(true); // thick1, thick2, thick3 ��ư Ȱ��ȭ
        }
        Debug.Log("Thick buttons are now visible");
    }

    void HideThickButtons()
    {
        foreach (var button in thickButtons)
        {
            button.SetActive(false); // thick1, thick2, thick3 ��ư ��Ȱ��ȭ
        }
    }
}
