using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinMenuHover : MonoBehaviour
{
    public LinePen linePen;


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
    

}
