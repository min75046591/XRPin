using UnityEngine;
using NRKernal;

public class FingerMenuInteraction : MonoBehaviour
{
    public HandEnum handEnum;
    public GameObject[] menus; // ���� ���� �޴� ������Ʈ �迭
    public float tolerance = 50f; // ��ġ �񱳸� ���� ��� ���� (ȭ�� ��ǥ �󿡼��� �Ÿ�)
    private GameObject lastExecutedMenu; // ���������� ����� �޴��� ����

    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        // �հ��� ���� ȭ�� ��ǥ (x, y) ��������
        Vector3 fingerScreenPos = Camera.main.WorldToScreenPoint(pose.position);

        // �� �޴��� ȭ�� ��ǥ (x, y)�� �հ��� �� ��ǥ�� ��
        foreach (var menu in menus)
        {
            Vector3 menuScreenPos = Camera.main.WorldToScreenPoint(menu.transform.position);

            // x, y ��ǥ�� tolerance ���� �ȿ� �ִ��� Ȯ�� (z�� ����)
            if (Mathf.Abs(fingerScreenPos.x - menuScreenPos.x) <= tolerance &&
                Mathf.Abs(Mathf.Abs(fingerScreenPos.y) - menuScreenPos.y) <= tolerance)
            {
                // �޴��� �̹� ���� ���� �ƴϰų�, ������ ����� �޴��� �ٸ��� ����
                if (lastExecutedMenu != menu)
                {
                    ExecuteMenu(menu);
                    lastExecutedMenu = menu; // ���������� ������ �޴� ������Ʈ
                }
            }
            else if (lastExecutedMenu == menu)
            {
                // �հ����� �޴��� ���� ��� �ʱ�ȭ
                lastExecutedMenu = null;
            }
        }
    }

    void ExecuteMenu(GameObject menu)
    {
        // �޴� ���� ���� (��: �ش� �޴� Ȱ��ȭ �Ǵ� Ư�� ��� ����)
        menu.SetActive(true);
        Debug.Log($"{menu.name} Menu Executed!");
    }
}
