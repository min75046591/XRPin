using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDCirclePenCtrl : BasePenCtrl
{
    private BasePen pen;
    public float defaultDistance = 0.5f;
    public Transform CenterCamera;
    private Vector3 zeroVector = new Vector3(0, 0, 0);
    private Vector3 baseVector = new Vector3(0, 0, 0);
    private Vector3[] planeDirections = {
        Vector3.forward,       // Front
        Vector3.back,          // Back
        Vector3.left,          // Left
        Vector3.right,         // Right
    };

    public override void SelectPen(BasePen selectedPen)
    {
        this.pen = selectedPen;
    }


    public override void CalculatePoint(Vector3 indexTipPos)
    {
        this.pen.PenTransform.position = CalculateBaseVector(indexTipPos);
    }

    public override void ResetPoint()
    {
        this.pen.PenTransform.position = zeroVector;
    }

    public override void StartDraw()
    {
        this.pen.StartDraw();
    }

    public override void StopDraw()
    {
        this.pen.StopDraw();
    }

    public override void ChangeLineWidth(float lineWidth)
    {
        this.pen.ChangeLineWidth(lineWidth);
    }

    public override void ChangeColor(Material color)
    {
        this.pen.ChangeColor(color);
    }

    private Vector3 CalculateOffset(Transform cameraTransform)
    {
        Vector3 position = cameraTransform.forward * defaultDistance;
        return position;
    }

    private Quaternion CalculateRotation(Transform cameraTransform)
    {
        Quaternion rotation = cameraTransform.rotation;
        return rotation;
    }

    private Vector3 CalculateBaseVector(Vector3 indexTipPos)
    {
        // Determine the best plane direction based on CenterCamera.forward
        Vector3 bestPlaneDirection = GetBestPlaneDirection();

        // Define the target plane using the selected best plane direction
        Vector3 planePoint = bestPlaneDirection * defaultDistance;
        Vector3 planeNormal = bestPlaneDirection;

        // Calculate the projection of the finger position onto the selected plane
        Vector3 vectorToPlane = indexTipPos - planePoint;
        float distanceToPlane = Vector3.Dot(vectorToPlane, planeNormal);

        return indexTipPos - distanceToPlane * planeNormal;
    }

    private Vector3 GetBestPlaneDirection()
    {
        Vector3 forward = CenterCamera.forward;
        float bestDotProduct = float.MinValue;
        Vector3 bestDirection = Vector3.forward;

        // Find the plane direction closest to the camera's forward vector
        foreach (Vector3 direction in planeDirections)
        {
            float dotProduct = Vector3.Dot(forward, direction);
            if (dotProduct > bestDotProduct)
            {
                bestDotProduct = dotProduct;
                bestDirection = direction;
            }
        }

        return bestDirection;
    }

}
