using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using System;
using Unity.VisualScripting;

public class StationeryController : MonoBehaviour
{
    public HandEnum rightHandEnum;
    public HandEnum leftHandEnum;
    public BasePen LinePen;
    public BasePenCtrl TwoDPenCtrl;
    public BasePenCtrl ThreeDPenCtrl;
    public float lineWidth = 0.005f;

    public BaseEraser TwoDLineEraser;
    public BaseEraserCtrl TwoDEraserCtrl;
    public BaseEraserCtrl ThreeDEraserCtrl;
    public float circleDiameter = 0.005f;

    public List<Material> colorMaterials;

    private BasePenCtrl CurrentPenController;
    private BaseEraserCtrl CurrentEraserController;

    private bool drawingMode;
    private bool removingMode;
    private bool twoDMode = true;
    private bool threeDMode = false;
    private HandGesture prevRightGesture = HandGesture.None;
    private HandGesture prevLeftGesture = HandGesture.None;
    private bool increasingMode = false;
    private bool decreasingMode = true;
    void Awake()
    {
        // initialize Pen
        this.SetPenCtrl(ThreeDPenCtrl);
        this.SelectPen(LinePen);
        this.CurrentPenController.ChangeColor(colorMaterials[0]);
        this.ChangeLineWidth(lineWidth);
        drawingMode = true;

        // initialize Eraser
        this.SetEraserCtrl(ThreeDEraserCtrl);
        this.SelectEraser(TwoDLineEraser);
        this.ChangeEraserDiameter(circleDiameter);
        removingMode = false;
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning) return;
        var rightHandState = NRInput.Hands.GetHandState(rightHandEnum);
        JudgeRightGesture(rightHandState);


        var leftHandState = NRInput.Hands.GetHandState(leftHandEnum);
        JudgeLeftGesture(leftHandState);
    }

    public void DisplayDrawingPoint(HandState handState)
    {
        if (IsVisible(drawingMode, handState))
        {
            var pose = handState.GetJointPose(HandJointID.IndexTip);
            this.CurrentPenController.CalculatePoint(pose.position);
        }
        else this.CurrentPenController.ResetPoint();
    }

    public void DisplayRemovingPoint(HandState handState)
    {
        if (IsVisible(removingMode, handState))
        {
            var pose = handState.GetJointPose(HandJointID.IndexTip);
            this.CurrentEraserController.CalculatePoint(pose.position);
        }
        else this.CurrentEraserController.ResetPoint();
    }

    private void JudgeRightGesture(HandState state)
    {
        HandGesture gesture = state.currentGesture;
        DisplayDrawingPoint(state);
        DisplayRemovingPoint(state);
        JudgeDrawing(gesture);
        JudgeRemoving(gesture);
        JudgeModeSwitching(gesture, prevRightGesture);
        prevRightGesture = gesture;
    }

    private void JudgeLeftGesture(HandState state)
    {
        HandGesture gesture = state.currentGesture;
        JudgeIncreasingOrDecreasing(gesture);
        ControlThickness(gesture);
        prevLeftGesture = gesture;
    }

    private void JudgeIncreasingOrDecreasing(HandGesture gesture)
    {
        if (prevLeftGesture != HandGesture.Grab && gesture == HandGesture.Grab)
        {
            this.increasingMode = !this.increasingMode;
            this.decreasingMode = !this.decreasingMode;
        }
    }

    private void ControlThickness(HandGesture gesture)
    {
        if (gesture != HandGesture.Grab) return;
        if (increasingMode && drawingMode) AddLineDelta(0.0005f);
        else if (increasingMode && removingMode) AddDiameterDelta(0.0005f);
        else if (decreasingMode && drawingMode) AddLineDelta(-0.0005f);
        else if (decreasingMode && removingMode) AddDiameterDelta(-0.0005f);
    }

    private void JudgeDrawing(HandGesture gesture)
    {
        if (CanStart(drawingMode, gesture)) this.CurrentPenController.StartDraw();
        else this.CurrentPenController.StopDraw();
    }

    private void JudgeRemoving(HandGesture gesture)
    {
        if (CanStart(removingMode, gesture)) this.CurrentEraserController.StartRemoving();
    }

    private void JudgeModeSwitching(HandGesture gesture, HandGesture prevGesture)
    {
        if (CanSwitching(gesture, prevGesture))
        {
            drawingMode = !drawingMode;
            removingMode = !removingMode;
        }
    }


    private bool IsVisible(bool mode, HandState handState)
    {
        return mode && (handState.currentGesture == HandGesture.Point || handState.currentGesture == HandGesture.OpenHand
        || handState.currentGesture == HandGesture.Grab);
    }

    private bool CanStart(bool mode, HandGesture gesture)
    {
        return mode && gesture == HandGesture.Point;
    }

    private bool CanSwitching(HandGesture gesture, HandGesture prevGesture)
    {
        return prevGesture != HandGesture.Grab && gesture == HandGesture.Grab;
    }


    public void SetPenCtrl(BasePenCtrl penCtrl)
    {
        this.CurrentPenController = penCtrl;
    }

    public void SelectPen(BasePen pen)
    {
        this.TwoDPenCtrl.SelectPen(pen);
        this.ThreeDPenCtrl.SelectPen(pen);
    }

    public void SetEraserCtrl(BaseEraserCtrl eraserCtrl)
    {
        this.CurrentEraserController = eraserCtrl;
    }

    public void SelectEraser(BaseEraser eraser)
    {
        //this.TwoDEraserCtrl.SelectEraser(eraser);
        this.ThreeDEraserCtrl.SelectEraser(eraser);
    }

    public void AddLineDelta(float lineDelta)
    {
        if (this.lineWidth + lineDelta <= 0f) return;
        this.lineWidth += lineDelta;
        this.CurrentPenController.ChangeLineWidth(this.lineWidth);
    }

    public void ChangeLineWidth(float lineWidth)
    {
        if (lineWidth <= 0f) return;
        this.lineWidth = lineWidth;
        this.CurrentPenController.ChangeLineWidth(this.lineWidth);
    }

    public void AddDiameterDelta(float diameterDelta)
    {
        if (this.circleDiameter + diameterDelta <= 0f) return;
        this.circleDiameter += diameterDelta;
        this.CurrentEraserController.ChangeCircleDiameter(circleDiameter);
    }

    public void ChangeEraserDiameter(float circleDiameter)
    {
        if (circleDiameter <= 0f) return;
        this.circleDiameter = circleDiameter;
        this.CurrentEraserController.ChangeCircleDiameter(circleDiameter);
    }

    public void ChangeColor(Material colorMaterial)
    {
        this.CurrentPenController.ChangeColor(colorMaterial);
    }


    public void ConvertToDrawingMode()
    {
        this.drawingMode = true;
        this.removingMode = false;
    }
    public void ConvertToRemovingMode()
    {
        this.drawingMode = false;
        this.removingMode = true;
    }

    public void RemoveAll()
    {
        this.CurrentEraserController.RemoveAll();
    }
}
