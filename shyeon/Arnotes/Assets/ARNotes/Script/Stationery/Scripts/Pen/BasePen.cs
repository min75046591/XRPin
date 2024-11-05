using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePen : MonoBehaviour, IPen
{
    public abstract void StartDraw();
    public abstract void StopDraw();
    public abstract void ChangeLineWidth(float lineWidth);
    public abstract void ChangeColor(Material color);
    public Transform PenTransform { get { return transform; } }
}
