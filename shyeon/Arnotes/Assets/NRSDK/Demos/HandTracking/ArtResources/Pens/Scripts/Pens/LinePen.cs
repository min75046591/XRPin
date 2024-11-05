// 여러 라인을 그릴 수 있게 변경
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePen : BasePen
{
    public GameObject lineRendererPrefab;
    public Transform penPoint;
    public float lineWidth = 0.005f;
    public float lineLifeTime = 8f;

    private GameObject m_LineRendererObj;
    private LineRenderer m_LineRenderer;
    private List<Vector3> m_WorldPosList = new List<Vector3>();

    private const float MIN_LINE_SEGMENT = 0.01f;
    private bool isDrawingNewLine = true; // 새 라인 시작 여부를 추적

    private void Update()
    {
        if (IsDrawing)
        {
            Vector3 pos = penPoint.position;

            // 새 라인이 시작되면 새로운 LineRenderer 생성
            if (isDrawingNewLine || m_LineRendererObj == null)
            {
                CreateColoredLine();
                isDrawingNewLine = false;
            }

            // 이전 점과의 거리가 짧으면 추가하지 않음
            if (m_WorldPosList.Count > 1 && Vector3.Distance(pos, m_WorldPosList[m_WorldPosList.Count - 1]) < MIN_LINE_SEGMENT)
                return;

            Draw(pos);
        }
        else
        {
            // IsDrawing이 꺼질 때, 라인을 끊도록 플래그 설정
            isDrawingNewLine = true;

            // 사용자가 펜을 멈췄을 때, 기존 라인 삭제
            //if (m_LineRendererObj != null)
            //{
            //    DelayClearLine();
            //}
        }
    }

    private void CreateColoredLine()
    {
        m_LineRendererObj = Instantiate(lineRendererPrefab, this.transform);
        m_LineRendererObj.SetActive(true);
        m_LineRenderer = m_LineRendererObj.GetComponent<LineRenderer>();
        m_LineRenderer.numCapVertices = 8;
        m_LineRenderer.numCornerVertices = 8;
        m_LineRenderer.startWidth = lineWidth;
        m_LineRenderer.endWidth = lineWidth;

        // 새 라인 시작 시 위치 리스트 초기화
        m_WorldPosList.Clear();
    }

    private void Draw(Vector3 pos)
    {
        m_WorldPosList.Add(pos);
        m_LineRenderer.positionCount = m_WorldPosList.Count;
        m_LineRenderer.SetPositions(m_WorldPosList.ToArray());
    }

    private void DelayClearLine()
    {
        if (m_LineRendererObj)
        {
            m_LineRendererObj.transform.SetParent(null);
            m_LineRendererObj.AddComponent<DelayAutoDestroySelf>().DestroySelfWithDelay(lineLifeTime);
        }
        m_LineRendererObj = null;
    }
}
