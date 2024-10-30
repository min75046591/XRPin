using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class FollowUserView : MonoBehaviour
{
    public Transform headsetTransform; // ����� ������ Transform
    public float distanceFromUser = 1.0f; // ����� �տ� ������ �Ÿ�

    void Update()
    {
        if (headsetTransform == null)
        {
            headsetTransform = NRSessionManager.Instance.NRHMDPoseTracker.centerCamera.transform;
        }

        // ������� �տ� ��ġ ����
        Vector3 targetPosition = headsetTransform.position + headsetTransform.forward * distanceFromUser;
        transform.position = targetPosition;

        // ����ڸ� ���ϵ��� ȸ��
        transform.rotation = Quaternion.LookRotation(transform.position - headsetTransform.position);
    }
}