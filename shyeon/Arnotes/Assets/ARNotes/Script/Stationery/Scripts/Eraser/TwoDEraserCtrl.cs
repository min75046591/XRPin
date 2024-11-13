using NRKernal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDEraserCtrl : BaseEraserCtrl
{
    private BaseEraser baseEraser;
    private Vector3 zeroVector = new Vector3(0, 0, 0);



    public override void CalculatePoint(Vector3 indexTipPos)
    {
        indexTipPos.z = 0.5f;
        baseEraser.EraserTransform.position = indexTipPos;
    }
    public override void ResetPoint()
    {
        baseEraser.EraserTransform.position = zeroVector;
    }
    public override void SelectEraser(BaseEraser eraser)
    {
        this.baseEraser = eraser;
    }

    public override void StartRemoving()
    {
        baseEraser.StartRemoving();
    }

    public override void ChangeCircleDiameter(float diameter)
    {
        baseEraser.ChangeCircleDiameter(diameter);
    }

    public override void RemoveAll()
    {
        baseEraser.RemoveAll();
    }

}
