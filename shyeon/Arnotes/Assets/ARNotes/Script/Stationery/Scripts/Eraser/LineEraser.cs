using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEraser : BaseEraser
{

    public Transform erasePoint;
    public LineRendererHolder lineRendererHolder;

    private float circleDiameter = 0.005f;

    public override void StartRemoving()
    {
        List<LineRenderer> lineRenderers = lineRendererHolder.GetLineRenderers();
        for (int i = lineRenderers.Count - 1; i >= 0; i--)
        {
            if (IsHandNearLine(lineRenderers[i], erasePoint.position))
            {
                Destroy(lineRenderers[i].gameObject);
                lineRenderers.RemoveAt(i);
            }
        }
    }

    public override void ChangeCircleDiameter(float diameter)
    {
        this.circleDiameter = diameter;
        erasePoint.transform.localScale = new Vector3(circleDiameter, circleDiameter, circleDiameter);
    }

    private bool IsHandNearLine(LineRenderer line, Vector3 handPosition)
    {
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            Vector3 start = line.GetPosition(i);
            Vector3 end = line.GetPosition(i + 1);
            if (DistanceToLineSegment(handPosition, start, end) < circleDiameter * 0.5 + line.startWidth / 2)
            {
                return true;
            }
        }
        return false;
    }

    private float DistanceToLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 line = lineEnd - lineStart;
        float lineLength = line.magnitude;
        Vector3 lineDir = line / lineLength;

        float projectedLength = Vector3.Dot(point - lineStart, lineDir);
        projectedLength = Mathf.Clamp(projectedLength, 0, lineLength);

        Vector3 closestPoint = lineStart + lineDir * projectedLength;
        return Vector3.Distance(point, closestPoint);
    }

    public override void RemoveAll()
    {
        List<LineRenderer> lineRenderers = lineRendererHolder.GetLineRenderers();
        for (int i = lineRenderers.Count - 1; i >= 0; i--)
        {
            Destroy(lineRenderers[i].gameObject);
        }
        lineRenderers.Clear();
    }

}
