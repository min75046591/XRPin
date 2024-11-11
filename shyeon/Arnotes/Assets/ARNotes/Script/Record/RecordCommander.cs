using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordCommander : MenuCommander
{
    public RecordController recordController;
    /*
     * save 1 -> Yuan -> Capture
     * save 2 -> Minsu -> Record
     * save 3 -> Junwoo -> SaveCSV
     */
    public override void Command(string commandParam)
    {
        if (commandParam == "save1")
        {
            recordController.StartCapture();
        }
        else if (commandParam == "save2")
        {
            recordController.StartRecord();
        }
        else if (commandParam == "save3")
        {
            recordController.StartSaveCSV();
        }

    }
}
