using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEraser : MonoBehaviour, IEraser
{
    public abstract void StartRemoving();
    public abstract void ChangeCircleDiameter(float diameter);
    public Transform EraserTransform { get { return transform; } }
}
