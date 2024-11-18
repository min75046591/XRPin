using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NRKernal;
using Unity.VisualScripting;
using LitJson;

public class PinMenuHover : MonoBehaviour
{
    public MenuCommander menuCommander;
    public HandEnum handEnum;
    public RectTransform panelRectTransform;
    public Transform panel;
    public Image targetImage;
    public static Image currentImage;
    public float hoverTime = 0.5f;
    public float buttonHoverTime = 0.5f;
    public Camera nrealCamera;
    public float speed = 1f;
    public Image SubTargetImage;

    private Color originalTargetColor;
    private Vector3 initialSize = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 scaledSize = new Vector3(1.2f, 1.2f, 1.2f);
    public MainController mainController;

    private float hoverTimer = 0f;
    private bool isHovering = false;
    private GameObject currentHoveredButton = null;
    private float buttonHoverTimer = 0f;

    private static Pin currentPin;
    public LinePen linePen;

    public VideoLoader videoLoader; 

    void Start()
    {
        if (targetImage != null)
        {
            originalTargetColor = targetImage.color;
        }
    }

    void Update()
    {
        if (!NRInput.Hands.IsRunning)
            return;

        var handState = NRInput.Hands.GetHandState(handEnum);
        var pose = handState.GetJointPose(HandJointID.IndexTip);

        Vector3 screenPoint = nrealCamera.WorldToScreenPoint(pose.position);
        Vector2 pointerScreenPos = new Vector2(screenPoint.x, screenPoint.y);
        CheckHoverOnPanel(pointerScreenPos);
    }

    void CheckHoverOnPanel(Vector2 pointerScreenPos)
    {
        targetImage.transform.localScale = Vector3.Lerp(initialSize, scaledSize, hoverTimer * 3f);

        if (RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, pointerScreenPos, nrealCamera))
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverTime)
            {
                if (currentImage == null)
                {
                    currentImage = targetImage;
                }
                switch (targetImage.name)
                {
                    case "memo":
                        Debug.Log("memo activate");
                        List<LineObject> lineObject = GetPinLineObjects(currentPin);
                        Debug.Log(lineObject.Count);
                        DisplayCurrentPinLines(lineObject);
                        menuCommander.Command(targetImage.name);
                        ((MemoCommander)menuCommander).SetCurrentPin(currentPin);
                        break;
                    case "video":
                        Debug.Log("video activate");
                        string currentPinVideoPath = currentPin.getVideoPaths();
                        videoLoader.VideoLoadAndPlay(currentPinVideoPath);
                        //this.mainController.DisableReadUserInterface();
                        break;
                    case "complete":
                        Debug.Log("complete activate");
                        this.mainController.ChangePinStatusIntoCompleted(currentPin);
                        this.mainController.EnablePinGenerationMode();
                        this.mainController.DisableReadUserInterface();
                        break;
                    case "delete":
                        this.mainController.DeletePin(currentPin);
                        this.mainController.EnablePinGenerationMode();
                        this.mainController.DisableReadUserInterface();
                        break;
                    case "cancel":
                        Debug.Log("cancel activate");
                        this.mainController.EnablePinGenerationMode();
                        this.mainController.DisableReadUserInterface();
                        break;
                }
            }
        }
        else
        {
            hoverTimer = 0;
        }
    }

    public void DisplayCurrentPinLines(List<LineObject> lineObjects)
    {
        if (linePen != null)
        {
            foreach (LineObject lineObject in lineObjects)
            {
                // LineObject의 색상 및 두께를 설정
                //linePen.ChangeColor(new Material(Shader.Find("Standard")) { color = lineObject.Color });
                linePen.ChangeLineWidth(lineObject.Width);

                // 선 그리기
                linePen.DisplayLine(lineObject.GetPoints());

                // 리스트의 각 Vector3 값을 출력
                var points = lineObject.GetPoints();
                for (int i = 0; i < points.Count; i++)
                {
                    Debug.Log("Point " + i + ": " + points[i]);
                }
            }
        }
    }

    public List<LineObject> GetPinLineObjects(Pin currentPin)
    {
         return currentPin.GetMemos();
    }

    void InvertColor(Image image)
    {
        if (image != null)
        {
            Color invertedColor = new Color(1 - image.color.r, 1 - image.color.g, 1 - image.color.b, image.color.a);
            image.color = invertedColor;
        }
    }

    void ResetColor(Image image, Color originalColor)
    {
        if (image != null)
        {
            image.color = originalColor;
        }
    }

    public static void PassPin(Pin pin)
    {
        currentPin = pin;
        Debug.Log("pin Menu hover: " +  pin.GetPinName());
    }
}
