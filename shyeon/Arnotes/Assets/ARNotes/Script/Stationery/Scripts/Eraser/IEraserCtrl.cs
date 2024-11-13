using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEraserCtrl
{
    public abstract void CalculatePoint(Vector3 indexTipPos);
    public abstract void ResetPoint();
    public abstract void SelectEraser(BaseEraser eraser);
    public abstract void StartRemoving();
    public abstract void ChangeCircleDiameter(float diameter);
    public abstract void RemoveAll();
}
