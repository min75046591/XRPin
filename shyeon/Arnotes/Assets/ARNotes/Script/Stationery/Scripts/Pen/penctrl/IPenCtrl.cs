using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPenCtrl
{
    public abstract void CalculatePoint(Vector3 indexTipPos);
    public abstract void ResetPoint();
    public abstract void SelectPen(BasePen pen);
    public abstract void StartDraw();

    public abstract void StopDraw();

    public abstract void ChangeLineWidth(float lineWidth);
    public abstract void ChangeColor(Material color);
}
