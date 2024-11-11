using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class UICursor : MonoBehaviour
{
    public HandEnum handEnum;

    void Update()
    {
        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);
        Vector3 indexTipPosition = pose.position;
        transform.position = indexTipPosition;
    }


    //private Vector3 CalculatePosition(Transform cameraTransform)
    //{
    //    Vector3 position = cameraTransform.forward * defaultDistance;
    //    return position;
    //}

    //private Quaternion CalculateRotation(Transform cameraTransform)
    //{
    //    Quaternion rotation = cameraTransform.rotation;
    //    return rotation;
    //}
}
