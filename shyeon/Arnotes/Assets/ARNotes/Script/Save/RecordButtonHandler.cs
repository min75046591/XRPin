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
    private bool isVideoCaptureInitialized = false; // 초기화 상태 플래그

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
            Debug.LogError("RecordButtonHandler: 필요한 컴포넌트가 설정되지 않았습니다.");
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
                isVideoCaptureInitialized = true; // 초기화 완료 표시
                Debug.Log("NRVideoCapture 객체 생성 성공");
            }
            else
            {
                Debug.LogError("NRVideoCapture 객체 생성 실패");
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

    // 외부에서 호출할 수 있도록 public으로 설정
    public void StartRecording()
    {
        if (!isVideoCaptureInitialized || videoCapture == null || isRecording || isProcessing)
        {
            Debug.LogWarning("NRVideoCapture가 초기화되지 않았거나 이미 녹화 중이거나 작업이 진행 중입니다.");
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
            Debug.LogError("비디오 모드 설정에 실패했습니다.");
            isProcessing = false;
            recordButton.interactable = true;
            return;
        }

        Debug.Log("비디오 모드 시작 성공");
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

    // 외부에서 호출할 수 있도록 public으로 설정
    public void StopRecording()
    {
        if (videoCapture == null)
        {
            Debug.LogWarning("NRVideoCapture 객체가 초기화되지 않았습니다.");
            return;
        }

        if (!isRecording)
        {
            Debug.LogWarning("녹화가 이미 중지되었거나 녹화 중이 아닙니다.");
            return;
        }

        if (isProcessing)
        {
            Debug.LogWarning("작업이 진행 중입니다. 중복 중지 요청을 무시합니다.");
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
            Debug.Log("녹화를 중지합니다.");
            isRecording = false;
            buttonText.text = "Start";

            string savedPath = VideoSavePath;
            SaveVideoToGallery(savedPath);
        }
        else
        {
            Debug.LogError("녹화 중지에 실패했습니다.");
        }

        isProcessing = false;
        recordButton.interactable = true;
        videoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    private void SaveVideoToGallery(string videoPath)
    {
        if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
        {
            Debug.LogWarning("저장할 비디오 파일이 없습니다.");
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
                    Debug.Log("미디어 스캔 완료, 갤러리에 비디오가 추가되었습니다: " + videoPath);
                }));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("갤러리에 비디오 파일을 저장하는 중 오류 발생: " + e.Message);
        }
    }

    private void OnStoppedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("비디오 모드 중지 성공");
        }
        else
        {
            Debug.LogError("비디오 모드 중지에 실패했습니다.");
        }
    }

    private void OnRecordingStarted(NRVideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            buttonText.text = "Stop";
            isRecording = true;
            Debug.Log("녹화를 시작합니다.");
        }
        else
        {
            Debug.LogError("녹화 시작에 실패했습니다.");
            videoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
        }

        isProcessing = false;
        recordButton.interactable = true;
    }

    public void OnRecordButtonClicked()
    {
        if (!isVideoCaptureInitialized)
        {
            Debug.LogError("NRVideoCapture 인스턴스가 초기화되지 않았습니다.");
            return;
        }

        if (isProcessing)
        {
            Debug.LogWarning("이전 녹화 작업이 완료되지 않았습니다.");
            return;
        }

        if (isRecording)
        {
            Debug.Log("사용자 요청으로 녹화 중지 시도.");
            StartCoroutine(StopRecordingWithDelay());
        }
        else
        {
            Debug.Log("사용자 요청으로 녹화 시작 시도.");
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
