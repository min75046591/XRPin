//using UnityEngine;
//using NRKernal;

//public class FingerMenuInteraction : MonoBehaviour
//{
//    public HandEnum handEnum;
//    public GameObject[] menus; // ���� ���� �޴� ������Ʈ �迭
//    public float tolerance = 5f; // ��ġ �񱳸� ���� ��� ���� (ȭ�� ��ǥ �󿡼��� �Ÿ�)
//    private GameObject lastExecutedMenu; // ���������� ����� �޴��� ����

//    void Update()
//    {
//        if (!NRInput.Hands.IsRunning)
//            return;

//        var handState = NRInput.Hands.GetHandState(handEnum);
//        var pose = handState.GetJointPose(HandJointID.IndexTip);

//        // �հ��� ���� ȭ�� ��ǥ (x, y) ��������
//        Vector3 fingerScreenPos = Camera.main.WorldToScreenPoint(pose.position);

//        // �� �޴��� ȭ�� ��ǥ (x, y)�� �հ��� �� ��ǥ�� ��
//        foreach (var menu in menus)
//        {
//            Vector3 menuScreenPos = Camera.main.WorldToScreenPoint(menu.transform.position);

//            // x, y ��ǥ�� tolerance ���� �ȿ� �ִ��� Ȯ�� (z�� ����)
//            if (Mathf.Abs(Mathf.Abs(fingerScreenPos.x) - menuScreenPos.x) <= tolerance &&
//                Mathf.Abs(Mathf.Abs(fingerScreenPos.y) - menuScreenPos.y) <= tolerance)
//            {
//                // �޴��� �̹� ���� ���� �ƴϰų�, ������ ����� �޴��� �ٸ��� ����
//                if (lastExecutedMenu != menu)
//                {
//                    ExecuteMenu(menu);
//                    lastExecutedMenu = menu; // ���������� ������ �޴� ������Ʈ
//                }
//            }
//            else if (lastExecutedMenu == menu)
//            {
//                // �հ����� �޴��� ���� ��� �ʱ�ȭ
//                lastExecutedMenu = null;
//            }
//        }
//    }

//    void ExecuteMenu(GameObject menu)
//    {
//        // �޴� ���� ���� (��: �ش� �޴� Ȱ��ȭ �Ǵ� Ư�� ��� ����)
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
    public Image targetImage; // �����Ͱ� �÷����� �� �̹���
    public GameObject menu; // ������ �޴� ������Ʈ
    public float hoverTime = 1f; // �����Ͱ� �ӹ����� �ð� (1��)

    private float hoverTimer = 0f;
    private bool isHovering = false;

    void Start()
    {
        menu.SetActive(false); // �޴��� �ʱ⿡�� ���ܵ�
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        // ��Ʈ�ѷ� ��ġ�� ȭ�� ��ǥ ��������
        Vector3 pointerScreenPos = Camera.main.WorldToScreenPoint(pose.position);

        // PointerEventData ���� (��Ʈ�ѷ��� ȭ�� ��ǥ�� �������)
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = pointerScreenPos
        };

        // Raycast ��� ����Ʈ ����
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // �̹��� ���� �����Ͱ� �ִ��� Ȯ��
        isHovering = false;
        foreach (var result in results)
        {
            if (result.gameObject == targetImage.gameObject)
            {
                isHovering = true;
                break;
            }
        }

        // �����Ͱ� �̹��� ���� �ִ��� Ȯ���Ͽ� Ÿ�̸� ����
        if (isHovering)
        {
            hoverTimer += Time.deltaTime;

            // ������ �ð� �̻� Hover �� �޴� Ȱ��ȭ
            if (hoverTimer >= hoverTime)
            {
                ShowMenu();
            }
        }
        else
        {
            // �����Ͱ� �̹������� ����� Ÿ�̸� �ʱ�ȭ �� �޴� �����
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
