using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성한 메모 및 3D 선의 정보를 JSON 형식으로 변환하여 jsonData로 저장
public class SaveLineRenderer : MonoBehaviour
{
    public LineRendererHolder lineRendererHolder; // Holder에 접근하기 위한 public 변수
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // lineRenderers 정보를 JSON 형식으로 변환하는 메서드
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

            // LineRenderer의 각 점을 points에 저장
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineData.points.Add(lineRenderer.GetPosition(i));
            }

            lineDataList.Add(lineData);
        }

        // LineData 리스트를 JSON으로 변환
        return JsonUtility.ToJson(new LineDataWrapper { lines = lineDataList }, true);
    }

    public string GetJsonData()
    {
        return GetLineRenderersAsJson();
    }
}

// LineRenderer 데이터를 저장할 클래스
[System.Serializable]
public class LineData
{
    public List<Vector3> points;
    public Color Color;
    public float Width;
}

// 여러 개의 LineData 객체를 담는 래퍼 클래스
[System.Serializable]
public class LineDataWrapper
{
    public List<LineData> lines;
}
