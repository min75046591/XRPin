using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePen : BasePen
{
    public GameObject lineRendererPrefab;
    public Transform penPoint;

    public LineRendererHolder lineRendererHolder;

    private GameObject m_LineRendererObj;
    private LineRenderer m_LineRenderer;
    private List<Vector3> m_WorldPosList = new List<Vector3>();


    private float lineWidth = 0.005f;
    private const float MIN_LINE_SEGMENT = 0.01f;
    private Material color;

    public override void StartDraw()
    {
        if (m_LineRendererObj == null)
        {
            CreateColoredLine();
        }

        Vector3 pos = penPoint.position;
        if (m_WorldPosList.Count > 1 && Vector3.Distance(pos, m_WorldPosList[m_WorldPosList.Count - 1]) < MIN_LINE_SEGMENT)
            return;

        Draw(pos);   
    }



    public override void StopDraw()
    {
        if (m_LineRendererObj)
        {
            lineRendererHolder.AddLineRenderer(m_LineRenderer);
            m_LineRendererObj.transform.SetParent(null);
        }
        m_LineRendererObj = null;

        if (m_WorldPosList.Count != 0)
        {
            m_WorldPosList.Clear();
        }
    }

    public override void ChangeLineWidth(float lineWidth)
    {
        this.lineWidth = lineWidth;
        penPoint.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
    }

    public override void ChangeColor(Material color)
    {
        this.color = color;
    }

    private void CreateColoredLine()
    {
        m_LineRendererObj = Instantiate(lineRendererPrefab, this.transform);
        m_LineRendererObj.SetActive(true);
        m_LineRenderer = m_LineRendererObj.GetComponent<LineRenderer>();
        m_LineRenderer.numCapVertices = 8;
        m_LineRenderer.numCornerVertices = 8;
        m_LineRenderer.material = this.color;
        m_LineRenderer.startWidth = lineWidth;
        m_LineRenderer.endWidth = lineWidth;
    }

    private void Draw(Vector3 pos)
    {
        m_WorldPosList.Add(pos);
        m_LineRenderer.positionCount = m_WorldPosList.Count;
        m_LineRenderer.SetPositions(m_WorldPosList.ToArray());
    }
    public void DisplayLine(List<Vector3> positions)
    {
        if (m_LineRendererObj == null)
        {
            CreateColoredLine();
        }

        foreach (Vector3 pos in positions)
        {
            Draw(pos);
        }
        StopDraw();
    }
}
