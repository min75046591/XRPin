using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserThickCommander : MenuCommander
{
    public StationeryController stationeryController;
    public override void Command(string commandParam)
    {
        this.stationeryController.ConvertToRemovingMode();
        /*
            thick1 => eraser's circle Diameteris 0.001f
            thick2 => eraser's circle Diameter is 0.01f
            thick3 => eraser's circle Diameter is 0.1f
        */
        if (commandParam == "ethick1")
        {
            this.stationeryController.ChangeEraserDiameter(0.001f);
        }
        else if (commandParam == "ethick2")
        {
            this.stationeryController.ChangeEraserDiameter(0.01f);
        }
        else if (commandParam == "ethick3")
        {
            this.stationeryController.ChangeEraserDiameter(0.1f);
        }
    }
}
