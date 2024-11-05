using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererHolder : MonoBehaviour
{
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    
    public void AddLineRenderer(LineRenderer line)
    {
        lineRenderers.Add(line);
    }

    public List<LineRenderer> GetLineRenderers()
    {
        return this.lineRenderers;
    }



}
