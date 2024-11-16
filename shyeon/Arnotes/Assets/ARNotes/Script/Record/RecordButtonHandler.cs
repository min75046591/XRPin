using UnityEngine;
using TMPro;
using NRKernal.Record;
using NRKernal;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Android;

public class RecordButtonHandler : MonoBehaviour
{
    private NRVideoCapture videoCapture;
    public NRPreviewer previewer;
    private bool isRecording = false;
    private bool isProcessing = false;
    private bool isVideoCaptureInitialized = false; // 초기화 상태 플래그
    private bool isDestroyed = false; // 객체 파괴 상태 플래그
    private string videoSavePath;

    private void Start()
    {
        RequestStoragePermission();
    }

    public void RequestStoragePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Debug.Log("Storage permission requested.");
        }
        else
        {
            Debug.Log("Storage permission already granted.");
            Initialize();
        }
    }

    public void Initialize()
    {
        NRVideoCapture.CreateAsync(false, captureObject =>
        {
            if (captureObject != null && !isDestroyed)
            {
                videoCapture = captureObject;
                isVideoCaptureInitialized = true; // 초기화 완료 표시
                Debug.Log("NRVideoCapture 객체 생성 성공");
                StartRecording();
            }
            else
            {
                Debug.LogError("NRVideoCapture 객체 생성 실패");
            }
        });
    }

    public string VideoSavePath
    {
        get
        {
            if (string.IsNullOrEmpty(videoSavePath))
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filename = $"VID_{timeStamp}.mp4";
                videoSavePath = Path.Combine(Application.persistentDataPath, filename);
            }
            return videoSavePath;
        }
        set
        {
            videoSavePath = value;
        }
    }

    public void StartRecording()
    {
        if (!isVideoCaptureInitialized || videoCapture == null || isRecording || isProcessing)
        {
            Debug.LogWarning("NRVideoCapture is not initialized or recording is already in progress.");
            return;
        }

        isProcessing = true;
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

        Debug.Log("Starting video mode...");
        videoCapture.StartVideoModeAsync(cameraParameters, OnStartedVideoCaptureMode);
    }

    private void OnStartedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
    {
        if (!result.success || isDestroyed)
        {
            Debug.LogError("Failed to start video mode.");
            isProcessing = false;
            return;
        }

        Debug.Log("Video mode started successfully.");
        videoCapture.StartRecordingAsync(VideoSavePath, OnRecordingStarted);

        if (previewer != null)
        {
            previewer.SetData(videoCapture.PreviewTexture, true);
        }
    }

    public void StopRecording()
    {
        if (videoCapture == null || !isRecording || isDestroyed)
        {
            Debug.LogWarning("Cannot stop recording. Recording not in progress or NRVideoCapture not initialized.");
            return;
        }

        if (isProcessing)
        {
            Debug.LogWarning("Processing in progress. Ignoring stop request.");
            return;
        }

        isProcessing = true;
        Debug.Log("Stopping recording...");
        videoCapture.StopRecordingAsync(OnRecordingStopped);
    }

    private void OnRecordingStopped(NRVideoCapture.VideoCaptureResult result)
    {
        if (isDestroyed) return;

        if (result.success)
        {
            Debug.Log("Recording stopped successfully. Video saved at: " + VideoSavePath);
            isRecording = false;
        }
        else
        {
            Debug.LogError("Failed to stop recording.");
        }

        isProcessing = false;
        videoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    private void OnStoppedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
    {
        if (isDestroyed) return;

        if (result.success)
        {
            Debug.Log("Video mode stopped successfully.");
        }
        else
        {
            Debug.LogError("Failed to stop video mode.");
        }
    }

    private void OnRecordingStarted(NRVideoCapture.VideoCaptureResult result)
    {
        if (isDestroyed) return;

        if (result.success)
        {
            isRecording = true;
            Debug.Log("Recording started.");
        }
        else
        {
            Debug.LogError("Failed to start recording.");
            videoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
        }

        isProcessing = false;
    }

    private void OnDestroy()
    {
        isDestroyed = true; // 객체가 파괴되었음을 표시
        if (videoCapture != null)
        {
            if (isRecording)
            {
                StopRecording();
            }

            videoCapture.Dispose();
            Debug.Log("NRVideoCapture object disposed.");
        }
    }
}
