using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using NRKernal.Record;
using TMPro;


public class RecordController : MonoBehaviour
{
    public RecordButtonHandler recordButtonHandler; // RecordButtonHandler 인스턴스 참조

    public void StartCapture()
    {
        Debug.Log("StartCapture 호출");
    }

    public void StartRecord()
    {
        Debug.Log("StartRecord 호출");

        // RecordButtonHandler의 StartRecording() 메서드를 호출하여 녹화를 바로 시작
        if (recordButtonHandler != null)
        {
            recordButtonHandler.StartRecording(); // 녹화를 바로 시작
        }
        else
        {
            Debug.LogError("RecordButtonHandler가 설정되지 않았습니다.");
        }
    }

    public void StartSaveCSV()
    {
        Debug.Log("StartSaveCSV 호출");
    }
}