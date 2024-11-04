using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleControllerBar : MonoBehaviour
{
    public GameObject uiElement; // UI ��Ҹ� �����մϴ�.
    public Transform penpoint; // ��Ʈ�ѷ� ������Ʈ�� �����մϴ�.
    public float showThreshold = 2.0f; // UI�� ���̱� ������ Y ��ġ

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        // UI ��ҿ� ���� CanvasGroup ������Ʈ�� �����ɴϴ�.
        canvasGroup = uiElement.GetComponent<CanvasGroup>();

        // �ʱ� ���¸� �������� ����
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ʈ�ѷ��� Y ��ġ�� showThreshold �̻��� �� UI�� ǥ��
        if (penpoint.position.y > showThreshold)
        {
            canvasGroup.alpha = 1; // UI ���̱�
        }
        else
        {
            canvasGroup.alpha = 0; // UI �����
        }
    }
}
