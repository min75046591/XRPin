using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEraser
{
    public abstract void StartRemoving();
    public abstract void ChangeCircleDiameter(float diameter);
    Transform EraserTransform { get; }
}
