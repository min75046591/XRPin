using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using NRKernal.Record;
using TMPro;


public class RecordController : MonoBehaviour
{
    public RecordButtonHandler recordButtonHandler; // RecordButtonHandler �ν��Ͻ� ����

    public void StartCapture()
    {
        Debug.Log("StartCapture ȣ��");
    }

    public void StartRecord()
    {
        Debug.Log("StartRecord ȣ��");

        // RecordButtonHandler�� StartRecording() �޼��带 ȣ���Ͽ� ��ȭ�� �ٷ� ����
        if (recordButtonHandler != null)
        {
            recordButtonHandler.StartRecording(); // ��ȭ�� �ٷ� ����
        }
        else
        {
            Debug.LogError("RecordButtonHandler�� �������� �ʾҽ��ϴ�.");
        }
    }

    public void StartSaveCSV()
    {
        Debug.Log("StartSaveCSV ȣ��");
    }
}