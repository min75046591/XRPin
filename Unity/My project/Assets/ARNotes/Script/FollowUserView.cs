using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class FollowUserView : MonoBehaviour
{
    public Transform headsetTransform; // 사용자 헤드셋의 Transform
    public float distanceFromUser = 1.0f; // 사용자 앞에 고정될 거리

    void Update()
    {
        if (headsetTransform == null)
        {
            headsetTransform = NRSessionManager.Instance.NRHMDPoseTracker.centerCamera.transform;
        }

        // 사용자의 앞에 위치 고정
        Vector3 targetPosition = headsetTransform.position + headsetTransform.forward * distanceFromUser;
        transform.position = targetPosition;

        // 사용자를 향하도록 회전
        transform.rotation = Quaternion.LookRotation(transform.position - headsetTransform.position);
    }
}