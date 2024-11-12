using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성한 메모 및 3D 선의 정보를 JSON 형식으로 변환하여 jsonData로 저장
public class SaveLineRenderer : MonoBehaviour
{
    public LineRendererHolder lineRendererHolder; // Holder에 접근하기 위한 public 변수
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // lineRenderers 정보를 JSON 형식으로 변환하는 메서드
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

            // LineRenderer의 각 점을 points에 저장
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineData.AddPoint(lineRenderer.GetPosition(i));
            }
            lineDataList.Add(lineData);
        }

        // LineData 리스트를 JSON으로 변환
        return lineDataList;
    }

}