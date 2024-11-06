using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDPenCtrl : BasePenCtrl
{
    private BasePen pen;

    private Vector3 zeroVector = new Vector3(0, 0, 0);


    public override void SelectPen(BasePen selectedPen)
    {
        this.pen = selectedPen;
    }


    public override void CalculatePoint(Vector3 indexTipPos)
    {
        this.pen.PenTransform.position = indexTipPos;
    }

    public override void ResetPoint()
    {
        this.pen.PenTransform.position = zeroVector;
    }

    public override void StartDraw()
    {
        this.pen.StartDraw();
    }

    public override void StopDraw()
    {
        this.pen.StopDraw();
    }

    public override void ChangeLineWidth(float lineWidth)
    {
        this.pen.ChangeLineWidth(lineWidth);
    }

    public override void ChangeColor(Material color)
    {
        this.pen.ChangeColor(color);
    }
}
