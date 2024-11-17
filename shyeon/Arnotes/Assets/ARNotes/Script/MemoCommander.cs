using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoCommander : MenuCommander
{
    public InterfaceToggle toggle;
    public GameObject memoSaverGameObject;
    public MemoSaver memoSaver;
    public override void Command(string commandParam)
    {
        toggle.EnableStationery();
        memoSaverGameObject.SetActive(true);
    }

    public void SetCurrentPin(Pin pin)
    {
        memoSaver.SetCurrentPin(pin);
    }
}
