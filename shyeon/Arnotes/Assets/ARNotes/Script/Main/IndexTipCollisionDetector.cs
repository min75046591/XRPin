using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexTipCollisionDetector : MonoBehaviour
{
    public MainController mainController;
    private void OnTriggerEnter(Collider other)
    {
        GameObject pin = other.transform.parent?.gameObject;
        Pin p = mainController.FindPinByName(pin.name);
        Debug.Log(pin.name);
    }
}
