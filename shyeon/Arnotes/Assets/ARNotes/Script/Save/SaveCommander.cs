using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCommander : MenuCommander
{
    public SaveController saveController;
    /*
     * save 1 -> Yuan -> Capture
     * save 2 -> Minsu -> Record
     * save 3 -> Junwoo -> SaveCSV
     */
    public override void Command(string commandParam)
    {
        if(commandParam == "save1")
        {
            Debug.Log("123");
            saveController.StartCapture();
        }
        else if(commandParam == "save2")
        {
            Debug.Log("123");
            saveController.StartRecord();
        }
        else if(commandParam == "save3")
        {
            Debug.Log("123");
            saveController.StartSaveCSV();
        }

    }
}
