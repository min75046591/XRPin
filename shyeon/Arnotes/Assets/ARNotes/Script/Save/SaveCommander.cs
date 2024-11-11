using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCommander : MenuCommander
{
    public SaveManager SaveManager;
    public override void Command(string commandParam)
    {
        if (commandParam == "save1")
        {
            this.SaveManager.ActivateRecordMode();
        }
        else if (commandParam == "save2")
        {
            this.SaveManager.ActivateSave2Mode();
        }
        else if (commandParam == "save3")
        {
            this.SaveManager.ActivateSave3Mode();
        }
    }
}