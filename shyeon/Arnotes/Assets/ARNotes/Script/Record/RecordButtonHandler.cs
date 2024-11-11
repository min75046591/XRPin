using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NRKernal.Record;
using NRKernal;
using System.IO;
using System.Linq;
using System.Collections;
using System;
using UnityEngine.Android;

public class RecordButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Button recordButton;
    private NRVideoCapture videoCapture;
    public NRPreviewer previewer;
    private bool isRecording = false;
    private bool isProcessing = false;
    private bool isVideoCaptureInitialized = false; // �ʱ�ȭ ���� �÷���

    private void Start()
    {
        RequestStoragePermission();
        Initialize();
    }

    private void RequestStoragePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    private void Initialize()
    {
        if (buttonText == null || recordButton == null)
        {
            Debug.LogError("RecordButtonHandler: �ʿ��� ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }

        buttonText.text = "Start";

        recordButton.onClick.RemoveListener(OnRecordButtonClicked);
        recordButton.onClick.AddListener(OnRecordButtonClicked);

        NRVideoCapture.CreateAsync(false, captureObject =>
        {
            if (captureObject != null)
            {
                videoCapture = captureObject;
                isVideoCaptureInitialized = true; // �ʱ�ȭ �Ϸ� ǥ��
                Debug.Log("NRVideoCapture ��ü ���� ����");
            }
            else
            {
                Debug.LogError("NRVideoCapture ��ü ���� ����");
            }
        });
    }

    private string VideoSavePath
    {
        get
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"Record_Video_{timeStamp}.mp4";
            string folderPath = "/storage/emulated/0/Movies/Record";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return Path.Combine(folderPath, filename);
        }
    }

    private IEnumerator StartRecordingWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartRecording();
    }

    // �ܺο��� ȣ���� �� �ֵ��� public���� ����
    public void StartRecording()
    {
        if (!isVideoCaptureInitialized || videoCapture == null || isRecording || isProcessing)
        {
            Debug.LogWarning("NRVideoCapture�� �ʱ�ȭ���� �ʾҰų� �̹� ��ȭ ���̰ų� �۾��� ���� ���Դϴ�.");
            return;
        }

        isProcessing = true;
        recordButton.interactable = false;

        CameraParameters cameraParameters = new CameraParameters
        {
            hologramOpacity = 0.9f,
            blendMode = BlendMode.Blend,
            frameRate = 30,
            audioState = NRVideoCapture.AudioState.MicAudio
        };

        Resolution cameraResolution = NRVideoCapture.SupportedResolutions.ElementAt(NRVideoCapture.SupportedResolutions.Count() / 2);
        cameraParameters.cameraResolutionWidth = cameraResolution.width;
        cameraParameters.cameraResolutionHeight = cameraResolution.height;

        videoCapture.StartVideoModeAsync(cameraParameters, OnStartedVideoCaptureMode);
    }

    private void OnStartedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
    {
        if (!result.success)
        {
            Debug.LogError("���� ��� ������ �����߽��ϴ�.");
            isProcessing = false;
            recordButton.interactable = true;
            return;
        }

        Debug.Log("���� ��� ���� ����");
        videoCapture.StartRecordingAsync(VideoSavePath, OnRecordingStarted);

        if (previewer != null)
        {
            previewer.SetData(videoCapture.PreviewTexture, true);
        }
    }

    private IEnumerator StopRecordingWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StopRecording();
    }

    // �ܺο��� ȣ���� �� �ֵ��� public���� ����
    public void StopRecording()
    {
        if (videoCapture == null)
        {
            Debug.LogWarning("NRVideoCapture ��ü�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        if (!isRecording)
        {
            Debug.LogWarning("��ȭ�� �̹� �����Ǿ��ų� ��ȭ ���� �ƴմϴ�.");
            return;
        }

        if (isProcessing)
        {
            Debug.LogWarning("�۾��� ���� ���Դϴ�. �ߺ� ���� ��û�� �����մϴ�.");
            return;
        }

        isProcessing = true;
        recordButton.interactable = false;
        videoCapture.StopRecordingAsync(OnRecordingStopped);
    }

    private void OnRecordingStopped(NRVideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("��ȭ�� �����մϴ�.");
            isRecording = false;
            buttonText.text = "Start";

            string savedPath = VideoSavePath;
            SaveVideoToGallery(savedPath);
        }
        else
        {
            Debug.LogError("��ȭ ������ �����߽��ϴ�.");
        }

        isProcessing = false;
        recordButton.interactable = true;
        videoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    private void SaveVideoToGallery(string videoPath)
    {
        if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
        {
            Debug.LogWarning("������ ���� ������ �����ϴ�.");
            return;
        }

        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection");

                mediaScannerConnection.CallStatic("scanFile", context, new string[] { videoPath }, new string[] { "video/mp4" },
                new AndroidJavaRunnable(() =>
                {
                    Debug.Log("�̵�� ��ĵ �Ϸ�, �������� ������ �߰��Ǿ����ϴ�: " + videoPath);
                }));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("�������� ���� ������ �����ϴ� �� ���� �߻�: " + e.Message);
        }
    }

    private void OnStoppedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("���� ��� ���� ����");
        }
        else
        {
            Debug.LogError("���� ��� ������ �����߽��ϴ�.");
        }
    }

    private void OnRecordingStarted(NRVideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            buttonText.text = "Stop";
            isRecording = true;
            Debug.Log("��ȭ�� �����մϴ�.");
        }
        else
        {
            Debug.LogError("��ȭ ���ۿ� �����߽��ϴ�.");
            videoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
        }

        isProcessing = false;
        recordButton.interactable = true;
    }

    public void OnRecordButtonClicked()
    {
        if (!isVideoCaptureInitialized)
        {
            Debug.LogError("NRVideoCapture �ν��Ͻ��� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        if (isProcessing)
        {
            Debug.LogWarning("���� ��ȭ �۾��� �Ϸ���� �ʾҽ��ϴ�.");
            return;
        }

        if (isRecording)
        {
            Debug.Log("����� ��û���� ��ȭ ���� �õ�.");
            StartCoroutine(StopRecordingWithDelay());
        }
        else
        {
            Debug.Log("����� ��û���� ��ȭ ���� �õ�.");
            StartCoroutine(StartRecordingWithDelay());
        }
    }

    private void OnDestroy()
    {
        if (videoCapture != null)
        {
            if (isRecording)
            {
                StopRecording();
            }

            videoCapture.Dispose();
        }

        if (recordButton != null)
        {
            recordButton.onClick.RemoveListener(OnRecordButtonClicked);
        }
    }
}
