using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ��� �޸� �� 3D ���� ������ JSON �������� ��ȯ�Ͽ� jsonData�� ����
public class SaveLineRenderer : MonoBehaviour
{
    public LineRendererHolder lineRendererHolder; // Holder�� �����ϱ� ���� public ����
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // lineRenderers ������ JSON �������� ��ȯ�ϴ� �޼���
    private string GetLineRenderersAsJson()
    {
        lineRenderers = lineRendererHolder.GetLineRenderers();
        Debug.Log("#############################lineRenderers: " + lineRenderers);

        List<LineData> lineDataList = new List<LineData>();

        foreach (var lineRenderer in lineRenderers)
        {
            Debug.Log("#############################lineRenderer: " + lineRenderer);
            Debug.Log(lineRenderer);
            LineData lineData = new LineData
            {
                points = new List<Vector3>(),
                Color = lineRenderer.startColor,
                Width = lineRenderer.startWidth,
            };

            // LineRenderer�� �� ���� points�� ����
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineData.points.Add(lineRenderer.GetPosition(i));
            }

            lineDataList.Add(lineData);
        }

        // LineData ����Ʈ�� JSON���� ��ȯ
        return JsonUtility.ToJson(new LineDataWrapper { lines = lineDataList }, true);
    }

    public string GetJsonData()
    {
        return GetLineRenderersAsJson();
    }
}

// LineRenderer �����͸� ������ Ŭ����
[System.Serializable]
public class LineData
{
    public List<Vector3> points;
    public Color Color;
    public float Width;
}

// ���� ���� LineData ��ü�� ��� ���� Ŭ����
[System.Serializable]
public class LineDataWrapper
{
    public List<LineData> lines;
}
