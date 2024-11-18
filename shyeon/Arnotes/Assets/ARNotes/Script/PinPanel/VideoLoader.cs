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

        // �ܺ� ����� ���� ��û(�ȵ���̵� 10�ΰ�� �ʿ�)
        RequestExternalStoragePermission();

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoEnd;

    }

    // �� �Լ��� ȣ���ϸ� �ڵ� �����
    public void VideoLoadAndPlay(string videoFilePath)
    {
        Debug.Log("videoFilePath: " +  videoFilePath);

        if (string.IsNullOrEmpty(videoFilePath))
        {
            Debug.LogError("���� ���� ��� ���� ����");
            return;
        }

        //VideoFilePath = "�⺻���/file.mp4" ����
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

    // TODO: ó���� quad ��Ȱ��ȭ �صα�) 
    // quad�� smooth follower ������Ʈ �߰�

    // ��ũ�� ui Ȱ��ȭ
    private void GetScreenActive()
    {
        videoTransform = screen.transform;
        Pose headPose = NRFrame.HeadPose;
        //videoTransform.position = headPose.position + headPose.rotation * Vector3.forward * 0.2f;
        screen.SetActive(true);
        videoPlayer.Prepare();
    }

    //���� ����
    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();

    }
    // ���� ������ ��ũ�� ��Ȱ��ȭ
    private void OnVideoEnd(VideoPlayer source)
    {
        screen.SetActive(false);
        source.Stop();
        this.mainController.EnableReadUserInterface();

    }


    // �ȵ���̵� 10 �� ��� �б⾲�� ����� ���� ��� ���� Ȯ���ؾ���  
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