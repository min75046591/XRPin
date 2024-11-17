using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexTipCollisionDetector : MonoBehaviour
{
    public MainController mainController;


    private void OnTriggerEnter(Collider other)
    {
        if (!mainController.IsEnableGenarationMode()) return;
        if (!IsColisionWithPin(other.gameObject)) return;
        GameObject pin = other.transform.parent?.gameObject;
        Pin p = mainController.FindPinByName(pin.name);
        this.mainController.ChangePinStatusIntoWorking(p);
        this.mainController.DisablePinGenerationMode();
        this.mainController.EnableReadUserInterface();
        PinMenuHover.PassPin(p);
    }

    private bool IsColisionWithPin(GameObject currentTarget)
    {
        if (currentTarget.name == "Sphere" || currentTarget.name == "Cylinder" ) return true;
        return false;
    }
}
