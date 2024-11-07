using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuCommander : MonoBehaviour
{
    public abstract void Command(string commandParam);
}
