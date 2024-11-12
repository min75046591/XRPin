using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ��� �޸� �� 3D ���� ������ JSON �������� ��ȯ�Ͽ� jsonData�� ����
public class SaveLineRenderer : MonoBehaviour
{
    public LineRendererHolder lineRendererHolder; // Holder�� �����ϱ� ���� public ����
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // lineRenderers ������ JSON �������� ��ȯ�ϴ� �޼���
    public List<LineObject> GetLineObject()
    {
        lineRenderers = lineRendererHolder.GetLineRenderers();
        Debug.Log("#############################lineRenderers: " + lineRenderers);

        List<LineObject> lineDataList = new List<LineObject>();

        foreach (var lineRenderer in lineRenderers)
        {
            Debug.Log("#############################lineRenderer: " + lineRenderer);
            Debug.Log(lineRenderer);
            LineObject lineData = new LineObject
            (
                new List<Vector3>(),
                lineRenderer.startColor,
                lineRenderer.startWidth
            );

            // LineRenderer�� �� ���� points�� ����
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineData.AddPoint(lineRenderer.GetPosition(i));
            }
            lineDataList.Add(lineData);
        }

        // LineData ����Ʈ�� JSON���� ��ȯ
        return lineDataList;
    }

}