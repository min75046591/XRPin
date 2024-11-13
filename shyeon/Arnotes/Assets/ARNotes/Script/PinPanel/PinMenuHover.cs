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
        //Pin의 JSON을 불러왔기 때문에 JSON 의 videoPath를 통해서 받아오는 로직
    }
    public void SaveCurrentPin()
    {
        //색 바꾸는 로직
        //Pin을 삭제하는 로직
    }
    public void Cancel()
    {
        //Pin을 삭제하는 로직
    }
    

}
