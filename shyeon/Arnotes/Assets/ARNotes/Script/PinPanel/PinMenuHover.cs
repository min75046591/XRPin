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
                        break;
                    case "video":
                        Debug.Log("save activate");
                        break;
                    case "complete":
                        Debug.Log("complete activate");
                        break;
                    case "cancel":
                        Debug.Log("cancel activate");
                        break;
                }
            }
        }
        else
        {
            hoverTimer = 0;
        }
    }

    public void DisplayCurrentPinLine(List<Vector3> positions)
    {
        linePen.DisplayLine(positions);
    }

    public void DisplayCurrentVideo()
    {
        //Pin�� JSON�� �ҷ��Ա� ������ JSON �� videoPath�� ���ؼ� �޾ƿ��� ����

    }
    public void SaveCurrentPin()
    {
        //�� �ٲٴ� ����
        //Pin�� �����ϴ� ����
    }
    public void Cancel()
    {
        //Pin�� �����ϴ� ����
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
        Debug.Log(pin.GetPinName());
    }
}
