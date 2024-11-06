using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using System;
using Unity.VisualScripting;

public class StationeryController : MonoBehaviour
{
    public HandEnum handEnum;
    public BasePen LinePen;
    public BasePenCtrl TwoDPenCtrl;
    public BasePenCtrl ThreeDPenCtrl;
    public float lineWidth = 0.005f;

    public BaseEraser TwoDLineEraser;
    public BaseEraserCtrl TwoDEraserCtrl;
    public BaseEraserCtrl ThreeDEraserCtrl;
    public float circleDiameter = 0.005f;

    public List<Material> colorMaterials;
    private int colorIdx = 0;


    private BasePenCtrl CurrentPenController;
    private BaseEraserCtrl CurrentEraserController;


    // delete
    private Vector3 previousPosition;
    private float lastSwipeTime;
    private float swipeThreshold = 0.2f;
    private float timeThreshold = 0.5f;

    private bool drawingMode;
    private bool removingMode;
    private float lastModeSwitchTime = 0f;
    private bool twoDMode = true;
    private bool threeDMode = false;
    public float modeSwitchCooldown = 1.0f;


    void Start()
    {
        // initialize Pen
        this.SetPenCtrl(TwoDPenCtrl);
        this.SelectPen(LinePen);
        this.CurrentPenController.ChangeColor(colorMaterials[0]);
        this.ChangeLineWidth(lineWidth);
        drawingMode = true;

        // initialize Eraser
        this.SetEraserCtrl(TwoDEraserCtrl);
        this.SelectEraser(TwoDLineEraser);
        this.ChangeEraserDiameter(circleDiameter);
        removingMode = false;
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning) return;
        var handState = NRInput.Hands.GetHandState(handEnum);

        HandGesture gesture = handState.currentGesture;

        DisplayDrawingPoint(handState);
        DisplayRemovingPoint(handState);


        JudgeDrawing(gesture);
        JudgeRemoving(gesture);
        JudgeModeSwitching(gesture);

        // to delete
        TestLineWidth(gesture);
        //TestColorChange(gesture);
        TestDimensionConversion(gesture);
        TestEraserCircleDiameter(gesture);

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

    private void JudgeDrawing(HandGesture gesture)
    {
        if (CanStart(drawingMode, gesture)) this.CurrentPenController.StartDraw();
        else this.CurrentPenController.StopDraw();
    }

    private void JudgeRemoving(HandGesture gesture)
    {
        if (CanStart(removingMode, gesture)) this.CurrentEraserController.StartRemoving();
    }

    private void JudgeModeSwitching(HandGesture gesture)
    {
        if(CanSwitching(gesture))
        {
            drawingMode = !drawingMode;
            removingMode = !removingMode;
            lastModeSwitchTime = Time.time;
        }
    }

    private void TestDimensionConversion(HandGesture gesture)
    {
        if (gesture == HandGesture.System) ChangeThreeD();

        if(gesture == HandGesture.ThumbsUp) ChangeTwoD();
    }

    // delete
    private int DetectDirection()
    {
        // Get the current position of the index fingertip (or another joint)
        var handState = NRInput.Hands.GetHandState(handEnum);
        Pose indexTipPose = handState.GetJointPose(HandJointID.Palm);
        Vector3 currentPosition = indexTipPose.position;

        // Check if the swipe is detected within the time threshold
        if (Time.time - lastSwipeTime > timeThreshold)
        {

            float swipeDistance = Vector3.Distance(previousPosition, currentPosition);
            // If the movement is greater than the swipe threshold, detect a swipe
            if (swipeDistance > swipeThreshold)
            {
                // Calculate the direction of the swipe
                Vector3 direction = (currentPosition - previousPosition).normalized;

                // Check if the swipe is primarily in a horizontal or vertical direction
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0.0f)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                lastSwipeTime = Time.time;
                previousPosition = currentPosition;
            } 
        }
        return 0;
    }

    private bool IsVisible(bool mode, HandState handState)
    {
        return mode && (handState.currentGesture == HandGesture.Point || handState.currentGesture == HandGesture.OpenHand
        || handState.currentGesture == HandGesture.Grab || handState.currentGesture == HandGesture.Victory);
    }

    private bool CanStart(bool mode, HandGesture gesture)
    {
        return mode && gesture == HandGesture.Point;
    }

    private bool CanSwitching(HandGesture gesture)
    {
        return (Time.time - lastModeSwitchTime) > modeSwitchCooldown && gesture == HandGesture.Call;
    }

    private void TestLineWidth(HandGesture gesture)
    {
        if(drawingMode && gesture == HandGesture.Grab)
        {
            this.AddLineDelta(0.0001f);
        }
        else if(drawingMode && gesture == HandGesture.Victory)
        {
            this.AddLineDelta(-0.0001f);
        }
    }

    private void TestColorChange(HandGesture gesture)
    {
        if (gesture == HandGesture.ThumbsUp)
        {
            int colorOffset = DetectDirection();
            if (colorOffset == 0) return;
            colorIdx += colorOffset;
            if (colorIdx < 0) colorIdx = colorMaterials.Count - 1;
            else if (colorIdx == colorMaterials.Count) colorIdx = 0;
            this.CurrentPenController.ChangeColor(colorMaterials[colorIdx]);
        }
    }

    private void TestEraserCircleDiameter(HandGesture gesture)
    {
        if (removingMode && gesture == HandGesture.Grab)
        {
            this.AddDiameterDelta(0.0001f);
        }
        else if (removingMode && gesture == HandGesture.Victory)
        {
            this.AddDiameterDelta(-0.0001f);
        }
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
        this.TwoDEraserCtrl.SelectEraser(eraser);
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

    public void ChangeThreeD()
    {
        this.SetPenCtrl(ThreeDPenCtrl);
        this.SetEraserCtrl(ThreeDEraserCtrl);
        this.twoDMode = false;
        this.threeDMode = true;
    }

    public void ChangeTwoD()
    {
        this.SetPenCtrl(TwoDPenCtrl);
        this.SetEraserCtrl(TwoDEraserCtrl);
        this.twoDMode = true;
        this.threeDMode = false;
    }
}
