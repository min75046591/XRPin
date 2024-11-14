using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexTipCollisionDetector : MonoBehaviour
{
    public MainController mainController;


    private void OnTriggerEnter(Collider other)
    {
        if (!mainController.IsEnableGenarationMode()) return;
        if (IsColisionWithLight(other.gameObject)) return;
        GameObject pin = other.transform.parent?.gameObject;
        Pin p = mainController.FindPinByName(pin.name);
        this.mainController.DisablePinGenerationMode();
        this.mainController.UseReadPanel();
        this.mainController.EnableReadUserInterface();
        MenuHover.PassPin(p);
    }

    private bool IsColisionWithLight(GameObject currentTarget)
    {
        if (currentTarget.name == "light") return true;
        return false;
    }
}
