using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePinCommander : MenuCommander
{
    public StationeryController stationeryController;


    public override void Command(string commandParam)
    {
        stationeryController.RemoveAll();        
    }
}
