using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Android;
using NRKernal;

public class VideoLoader : MonoBehaviour
{
    public GameObject screen;
    public VideoPlayer videoPlayer;
    private string videoFilePath;
    private Transform videoTransform;
    public MainController mainController;

    private void Start()
    {
#if UNITY_ANDROID && UNITY_2021_2_OR_NEWER
        if (!Permission.HasUserAuthorizedPermission("android.permission.MANAGE_EXTERNAL_STORAGE"))
        {
            Permission.RequestUserPermission("android.permission.MANAGE_EXTERNAL_STORAGE");
        }
#endif

        // 외부 저장소 권한 요청(안드로이드 10인경우 필요)
        RequestExternalStoragePermission();

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoEnd;

    }

    // 이 함수만 호출하면 자동 실행됨
    public void VideoLoadAndPlay(string videoFilePath)
    {
        Debug.Log("videoFilePath: " +  videoFilePath);

        if (string.IsNullOrEmpty(videoFilePath))
        {
            Debug.LogError("비디오 파일 경로 전달 에러");
            return;
        }

        //VideoFilePath = "기본경로/file.mp4" 가정
        else if (File.Exists(videoFilePath))
        {
            videoPlayer.url = videoFilePath;
            Debug.Log("GetScreenActive : ===========================================");
            GetScreenActive();
        }
        else
        {
        }

    }

    // TODO: 처음에 quad 비활성화 해두기) 
    // quad에 smooth follower 컴포넌트 추가

    // 스크린 ui 활성화
    private void GetScreenActive()
    {
        videoTransform = screen.transform;
        Pose headPose = NRFrame.HeadPose;
        //videoTransform.position = headPose.position + headPose.rotation * Vector3.forward * 0.2f;
        screen.SetActive(true);
        videoPlayer.Prepare();
    }

    //비디오 실행
    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();

    }
    // 비디오 끝나면 스크린 비활성화
    private void OnVideoEnd(VideoPlayer source)
    {
        screen.SetActive(false);
        source.Stop();
        this.mainController.EnableReadUserInterface();

    }


    // 안드로이드 10 일 경우 읽기쓰기 명시적 권한 허용 여부 확인해야함  
    private void RequestExternalStoragePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

}