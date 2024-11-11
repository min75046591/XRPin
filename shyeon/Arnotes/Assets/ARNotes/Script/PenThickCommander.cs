using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenThickCommander : MenuCommander
{
    public StationeryController stationeryController;

    public override void Command(string commandParam)
    {
        this.stationeryController.ConvertToDrawingMode();
        /*
            thick1 => pen's line Width is 0.001f
            thick2 => pen's line Width is 0.01f
            thick3 => pen's line Width is 0.1f
        */
        if (commandParam == "pthick1")
        {
            this.stationeryController.ChangeLineWidth(0.001f);
        }
        else if (commandParam == "pthick2")
        {
            this.stationeryController.ChangeLineWidth(0.01f);
        }
        else if (commandParam == "pthick3")
        {
            this.stationeryController.ChangeLineWidth(0.05f);
        }
    }
}
